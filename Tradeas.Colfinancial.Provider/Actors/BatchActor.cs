using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using Tradeas.Colfinancial.Provider.Exceptions;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Colfinancial.Provider.Scrapers;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Actors
{
    public class BatchActor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BatchActor));
        private readonly ImportProcessor _importProcessor;
        private readonly BrokerTransactionScraper _brokerTransactionScraper;
        private readonly BatchProcessor _batchProcessor;
        private readonly TaskProcessor _taskProcessor;
        private readonly IConfiguration _configuration;
        private ImportMode _importMode;
        private readonly WebDriverFactory _webDriverFactory;
        
        public BatchActor(ImportProcessor importProcessor,
                           BrokerTransactionScraper brokerTransactionScraper,
                           BatchProcessor batchProcessor,
                           TaskProcessor taskProcessor,
                           IConfiguration configuration,
                           WebDriverFactory webDriverFactory)
        {
            _importProcessor = importProcessor;
            _brokerTransactionScraper = brokerTransactionScraper;
            _configuration = configuration;
            _batchProcessor = batchProcessor;
            _taskProcessor = taskProcessor;
            _importMode = ImportMode.Batch;
            _webDriverFactory = webDriverFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult Do(TransactionParameter transactionParameter)
        {
            var isBackOfficeException = false;
            var sleepInterval = 5;
            _importProcessor.PurgeTrackers(transactionParameter);
            while (true)
            {
                if (isBackOfficeException)
                {
                    Logger.Info($"Performing sleep for {sleepInterval} minutes");
                    Thread.Sleep(TimeSpan.FromMinutes(sleepInterval));
                    sleepInterval *= 2;
                    Logger.Info("awoken from sleep");
                }

                var workerCount = (!string.IsNullOrEmpty(transactionParameter.Symbol))
                    ? 1 : Convert.ToInt32(_configuration["WorkerCount"]);
                Logger.Info($"setting worker count {workerCount}");

                var webDrivers = new List<IWebDriver>();
                var tasks = new List<Task>();
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;
                _batchProcessor.SkipCounter = 0;
                for (var index = 0; index <= workerCount; index++)
                {
                    var batch = (!string.IsNullOrEmpty(transactionParameter.Symbol))
                        ? new List<Import> { new Import {Symbol = transactionParameter.Symbol }}
                        : _batchProcessor
                            .Process(_importMode)
                            .GetData<List<Import>>();
                    if (batch.Count == 0) continue;
                    if (index > 0)
                    {
                        Logger.Info($"sleeping for 30 seconds to allow breathing for previously logged sessions");
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                    }

                    var webDriver = _webDriverFactory.Create();
                    var task = Task
                        .Factory
                        .StartNew(() => _brokerTransactionScraper.Scrape(transactionParameter, batch, webDriver),cancellationToken);
                    var symbols = string.Join(",", batch.Select(b => b.Symbol));
                    webDrivers.Add(webDriver);
                    Logger.Info($"Thread id: {task.Id} will processing the following symbols: {symbols}");
                    LogicalThreadContext.Properties["thread-id"] = task.Id;
                    tasks.Add(task);
                }

                Logger.Info($"waiting for all complete, thread count: {tasks.Count}");
                isBackOfficeException = _taskProcessor
                    .Process(tasks, cancellationTokenSource)
                    .IsSuccessful.Value;
                
                Logger.Info("performing webdriver cleanup");
                foreach (var webDriver in webDrivers)
                {
                    webDriver.Quit();
                    webDriver.Dispose();
                }
                
                Logger.Info("performing task cleanup");
                foreach (var task in tasks)
                {
                    task.Dispose();
                }

                var isCompleted = _importProcessor.IsCompleted();
                if (isCompleted)
                    break;
                _importMode = ImportMode.Retry;
            }
            
            return new TaskResult {IsSuccessful = true};
        }
    }
}