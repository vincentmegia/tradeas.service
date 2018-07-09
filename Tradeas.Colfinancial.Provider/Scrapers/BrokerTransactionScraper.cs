using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;
using Tradeas.Colfinancial.Provider.Builders;
using Tradeas.Colfinancial.Provider.Exceptions;
using Tradeas.Colfinancial.Provider.Navigators;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Colfinancial.Provider.Simulators;
using Tradeas.Models;
using Tradeas.Repositories;

namespace Tradeas.Colfinancial.Provider.Scrapers
{
    /// <summary>
    /// Todo: review if objects can be put safely into IOC without being affected by multi threading
    /// </summary>
    public class BrokerTransactionScraper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerTransactionScraper));
        private readonly IImportRepository _importRepository;
        private readonly IImportTrackerRepository _importTrackerRepository;
        private readonly IImportHistoryRepository _importHistoryRepository;
        private readonly IBrokerTransactionRepository _brokerTransactionRepository;

        public BrokerTransactionScraper(IImportRepository importRepository,
                                        IBrokerTransactionRepository brokerTransactionRepository,
                                        IImportTrackerRepository importTrackerRepository,
                                        IImportHistoryRepository importHistoryRepository)
        {
            _importRepository = importRepository;
            _brokerTransactionRepository = brokerTransactionRepository;
            _importTrackerRepository = importTrackerRepository;
            _importHistoryRepository = importHistoryRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult Scrape(TransactionParameter transactionParameter)
        {
            //design, keep creating new instances to avoid threading issue
            //leter consider using actor based threads
            var importsHistoryResponse = _importHistoryRepository.GetByDate(DateTime.Now);
            var importsHistory = importsHistoryResponse.GetData<ImportHistory>();
            if (importsHistory == null)
            {
                var importTrackersResponse = _importTrackerRepository.GetAll();
                var importTrackers = importTrackersResponse.GetData<List<ImportTracker>>();
                foreach (var importTracker in importTrackers)
                {
                    _importTrackerRepository.DeleteAsync(importTracker);
                }
            }
            _importHistoryRepository.Add(new ImportHistory("broker-transactions", "broker.service"));
            
            
            var importsResponse = _importRepository.GetAll();
            var imports = importsResponse
                .Result
                .GetData<List<Import>>()
                .OrderBy(i => i.Symbol);

            var workerCount = 9;
            var webDrivers = new List<IWebDriver>();
            var batchSize = imports.Count() / workerCount;
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
                
                if (index == 10) Logger.Info($"last batch size: {batch.Count}");

                if (index > 0)
                {
                    Logger.Info($"sleeping for 30 seconds to allow breathing for previously logged sessions");
                    Thread.Sleep(TimeSpan.FromSeconds(30));    
                }

                var task = Task
                    .Factory
                    .StartNew(() => Process(transactionParameter, batch, webDrivers), cancellationToken);
                var symbols = string.Join(",", batch.Select(b => b.Symbol));
                Logger.Info($"Thread id: {task.Id} will processing the following symbols: {symbols}");
                LogicalThreadContext.Properties["thread-id"] = task.Id;
                tasks.Add(task);
            }

            Logger.Info($"waiting for all complete, thread count: {tasks.Count}");
            Task.WaitAll(tasks.ToArray());
            Logger.Info("performing webdriver cleanup");
            foreach (var webDriver in webDrivers)
            {
                webDriver.Quit();
                webDriver.Dispose();
            }

            Logger.Info("processing retry items");
            //perform retry items on a single thread
            var items = DeadQueue.Items();
            webDrivers.Clear();
            webDrivers.Add(WebDriverFactory.Create());
            var retryTask = Task
                .Factory
                .StartNew(() => Process(transactionParameter, items, webDrivers));
            Task.WaitAll(retryTask);
            Logger.Info("processing retry items  completed.");
            Logger.Info("broker transaction processing completed.");
            
            Logger.Info("Adding tracking to import history");
            return new TaskResult {IsSuccessful = true};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionParameter"></param>
        /// <param name="batch"></param>
        /// <param name="webDrivers"></param>
        /// <returns></returns>
        private TaskResult Process(TransactionParameter transactionParameter, 
                                   List<Import> batch, 
                                   List<IWebDriver> webDrivers)
        {
            var webDriver = WebDriverFactory.Create();
            
            try
            {
                var loginNavigator = new LoginNavigator(webDriver);
                var loginNavigateResult = loginNavigator.Navigate();

                var loginSimulator = new LoginSimulator(webDriver, transactionParameter);
                var loginSimulateResult = loginSimulator.Simulate();

                Logger.Info("initiating broker navigator");
                var brokerTabNavigator = new BrokerTabNavigator(webDriver);
                Logger.Info("broker navigation in progress");
                var tabNavigatorResult = brokerTabNavigator.Navigate();
                Logger.Info("initiating table scraper");
                var brokerTableScraper = new BrokerTableScraper(batch.ToList(),
                    new BrokerTransactionBuilder(),
                    new BrokerTransactionProcessor(_brokerTransactionRepository),
                    new BrokerTransactionSimulator(webDriver),
                    brokerTabNavigator,
                    webDriver,
                    _importTrackerRepository);
                var tableScraper = brokerTableScraper.Scrape();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                if (e.Message.Contains("Back-office is currently updating"))
                    throw new BackOfficeOfflineException("back office exception detected", e);
                //throw;
            }
            webDrivers.Add(webDriver);
            return new TaskResult {IsSuccessful = true};
        }
    }
}