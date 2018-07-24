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
    public class BrokerActor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerActor));
        private readonly ImportProcessor _importProcessor;
        private readonly BrokerTransactionScraper _brokerTransactionScraper;
        private readonly IConfiguration _configuration;
        
        public BrokerActor(ImportProcessor importProcessor,
                           BrokerTransactionScraper brokerTransactionScraper,
                           IConfiguration configuration)
        {
            _importProcessor = importProcessor;
            _brokerTransactionScraper = brokerTransactionScraper;
            _configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult Do(TransactionParameter transactionParameter)
        {
            var isBackOfficeException = false;
            while (true)
            {
                if (isBackOfficeException)
                {
                    Logger.Info("Performing sleep for mins");
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    Logger.Info("awoken from sleep");
                }
                    
                var imports = _importProcessor
                    .Process()
                    .GetData<List<Import>>();
                var workerCount = Convert.ToInt32(_configuration["WorkerCount"]);
                var webDrivers = new List<IWebDriver>();
                var batchSize = imports.Count / workerCount;
                if (batchSize == 0) batchSize = imports.Count;
                var skipCounter = 0;
                var tasks = new List<Task>();
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;
                for (var index = 0; index <= workerCount; index++)
                {
                    var batch = imports
                        .Skip(skipCounter)
                        .Take(batchSize)
                        .ToList();
                    skipCounter += batchSize;

                    if (batch.Count == 0) continue;
                    if (index > 0)
                    {
                        Logger.Info($"sleeping for 30 seconds to allow breathing for previously logged sessions");
                        Thread.Sleep(TimeSpan.FromSeconds(30));
                    }

                    var webDriver = WebDriverFactory.Create();
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
                
                try
                {
                    Task.WaitAll(tasks.ToArray());
                }
                catch (AggregateException ae)
                {
                    ae.Handle(x =>
                    {
                        if (x is BackOfficeOfflineException)
                        {
                            Logger.Error("Detected that backoffice is updating, cancelling all threads.");
                            cancellationTokenSource.Cancel();
                            isBackOfficeException = true;
                            return true;
                        }
                        return false;
                    });
                }
                
                Logger.Info("performing webdriver cleanup");
                foreach (var webDriver in webDrivers)
                {
                    webDriver.Quit();
                    webDriver.Dispose();
                }

                var isCompleted = _importProcessor.IsCompleted();
                if (isCompleted)
                    break;
            }
            
            return new TaskResult {IsSuccessful = true};
        }
    }
}