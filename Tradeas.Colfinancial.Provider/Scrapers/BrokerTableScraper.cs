using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Tradeas.Colfinancial.Provider.Builders;
using Tradeas.Colfinancial.Provider.Navigators;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Colfinancial.Provider.Simulators;
using Tradeas.Models;
using Tradeas.Repositories;

namespace Tradeas.Colfinancial.Provider.Scrapers
{
    public class BrokerTableScraper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerTableScraper));
        private readonly IWebDriver _webDriver;
        private readonly List<Import> _imports;

        private readonly IImportTrackerRepository _importTrackerRepository; //consider if repository layer is correct in this layer

        private readonly BrokerTransactionBuilder _brokerTransactionBuilder;
        private readonly BrokerTransactionProcessor _brokerTransactionProcessor;
        private readonly BrokerTransactionSimulator _brokerTransactionSimulator;
        private readonly BrokerTabNavigator _brokerTabNavigator;

        public BrokerTableScraper(List<Import> imports,
                                  BrokerTransactionBuilder brokerTransactionBuilder,
                                  BrokerTransactionProcessor brokerTransactionProcessor,
                                  BrokerTransactionSimulator brokerTransactionSimulator,
                                  BrokerTabNavigator brokerTabNavigator,
                                  IWebDriver webDriver,
                                  IImportTrackerRepository importTrackerRepository)
        {
            _webDriver = webDriver;
            _imports = imports;
            _importTrackerRepository = importTrackerRepository;
            _brokerTransactionBuilder = brokerTransactionBuilder;
            _brokerTransactionProcessor = brokerTransactionProcessor;
            _brokerTransactionSimulator = brokerTransactionSimulator;
            _brokerTabNavigator = brokerTabNavigator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Result> Scrape()
        {

            foreach (var import in _imports)
            {
                LogicalThreadContext.Properties["symbol"] = import.Symbol;
                var importTrackerResponse = await _importTrackerRepository.GetAll();
                var exportedList = importTrackerResponse
                    .GetData<List<ImportTracker>>()
                    .Where(x => x.Symbol != "Retry");

                Logger.Info($"processing symbol: {import.Symbol}");
                var importTracker = new ImportTracker(import.Symbol);

                if (exportedList.Contains(importTracker))
                {
                    Logger.Info($"already been processed. skipping");
                    continue;
                }
                
                try
                {
                    var transactionSimulatorResult = await _brokerTransactionSimulator.Simulate(import.Symbol);
                    
                    var fluentWait = new DefaultWait<IWebDriver>(_webDriver)
                    {
                        Timeout = TimeSpan.FromSeconds(30),
                        PollingInterval = TimeSpan.FromMilliseconds(250)
                    };
                    fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                    var rows = fluentWait
                        .Until(x=>x.FindElements(By.XPath("/html/body/form/table/tbody/tr")));

                    if (rows.Count > 0)
                    {
                        Logger.Info("initiating transaction builder");
                        var brokerTransactions = _brokerTransactionBuilder.Build(rows.ToList(), import.Symbol);
                        Logger.Info("initiating transaction process");
                        await _brokerTransactionProcessor.Process(brokerTransactions);
                        _brokerTransactionBuilder.Transactions.Clear();

                        importTracker.Status = "Success";
                        Logger.Info($"broker transactions for completed for symbol: {import.Symbol}");
                    }
                    else
                    {
                        importTracker.Status = "None";
                        Logger.Info($"no broker transactions found");
                    }
                }
                
                catch (Exception e)
                {
                    Logger.Warn($"An issue trying to download broker transactions found, adding to retry queue for later processing", e);
                    DeadQueue.Add(import);
                    importTracker.Status = "Retry";
                }
                
                //switch back to main frame to prevent
                await _brokerTabNavigator.NavigateHeaderFrame();
                await _brokerTabNavigator.Navigate(true);
                await _importTrackerRepository.PostAsync(importTracker);
            }

            return new TaskResult{IsSuccessful = true};
        }
    }
}