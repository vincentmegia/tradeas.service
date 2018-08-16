using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Tradeas.Colfinancial.Provider.Builders;
using Tradeas.Colfinancial.Provider.Exceptions;
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

        private readonly BrokerTransactionBuilder _brokerTransactionBuilder;
        private readonly BrokerTransactionProcessor _brokerTransactionProcessor;
        private readonly BrokerTransactionSimulator _brokerTransactionSimulator;
        private readonly BrokerTabNavigator _brokerTabNavigator;
        private readonly ImportProcessor _importProcessor;

        public BrokerTableScraper(List<Import> imports,
                                  BrokerTransactionBuilder brokerTransactionBuilder,
                                  BrokerTransactionProcessor brokerTransactionProcessor,
                                  BrokerTransactionSimulator brokerTransactionSimulator,
                                  BrokerTabNavigator brokerTabNavigator,
                                  ImportProcessor importProcessor,
                                  IWebDriver webDriver)
        {
            _webDriver = webDriver;
            _imports = imports;
            _brokerTransactionBuilder = brokerTransactionBuilder;
            _brokerTransactionProcessor = brokerTransactionProcessor;
            _brokerTransactionSimulator = brokerTransactionSimulator;
            _brokerTabNavigator = brokerTabNavigator;
            _importProcessor = importProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult Scrape(TransactionParameter transactionParameter)
        {
            var counter = 0;
            foreach (var import in _imports)
            {
                counter++;
                Logger.Info($"processing {counter} out of {_imports.Count}");
                LogicalThreadContext.Properties["symbol"] = import.Symbol;
                var importTracker = new ImportTracker(import.Symbol);

                try
                {
                    //create new transaction parameter to avoid threading issues
                    var parameter = new TransactionParameter
                    {
                        Symbol = import.Symbol,
                        FromDate = transactionParameter.FromDate,
                        ToDate = transactionParameter.ToDate
                    };
                    _brokerTransactionSimulator.Simulate(parameter);

                    var fluentWait = new DefaultWait<IWebDriver>(_webDriver)
                    {
                        Timeout = TimeSpan.FromSeconds(30),
                        PollingInterval = TimeSpan.FromMilliseconds(250)
                    };
                    fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                    var rows = fluentWait
                        .Until(x => x.FindElements(By.XPath("/html/body/form/table/tbody/tr")));

                    if (rows.Count > 0)
                    {
                        Logger.Info("initiating transaction builder");
                        var brokerTransactions = _brokerTransactionBuilder.Build(rows.ToList(), parameter);
                        Logger.Info("initiating transaction process");
                        _brokerTransactionProcessor.Process(brokerTransactions);
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
                    importTracker.Status = "Retry";
                }
                
                //switch back to main frame to prevent
                _brokerTabNavigator.NavigateHeaderFrame();
                _brokerTabNavigator.Navigate(true);
                _importProcessor.AddTracker(importTracker);
            }

            Logger.Info($"broker table scraping completed.");
            return new TaskResult{IsSuccessful = true};
        }
    }
}