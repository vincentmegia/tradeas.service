using System;
using System.Collections.Generic;
using System.Linq;
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
    /// 
    /// </summary>
    public class BrokerTransactionScraper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerTransactionScraper));
        private readonly ITradeasRepository _tradeasRepository;
        private readonly ImportProcessor _importProcessor;

        public BrokerTransactionScraper(ITradeasRepository tradeasRepository,
                                        ImportProcessor importProcessor)
        {
            _tradeasRepository = tradeasRepository;
            _importProcessor = importProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult Scrape(TransactionParameter transactionParameter, 
                                 List<Import> batch,
                                 IWebDriver webDriver)
        {
            try
            {
                var loginNavigator = new LoginNavigator(webDriver);
                loginNavigator.Navigate();

                var loginSimulator = new LoginSimulator(webDriver, transactionParameter);
                loginSimulator.Simulate();

                Logger.Info("initiating broker navigator");
                var brokerTabNavigator = new BrokerTabNavigator(webDriver);
                Logger.Info("broker navigation in progress");
                brokerTabNavigator.Navigate();
                Logger.Info("initiating table scraper");
                
                var brokerTableScraper = new BrokerTableScraper(batch.ToList(),
                    new BrokerTransactionBuilder(),
                    new BrokerTransactionProcessor(_tradeasRepository),
                    new BrokerTransactionSimulator(webDriver),
                    brokerTabNavigator,
                    _importProcessor,
                    webDriver);
                brokerTableScraper.Scrape(transactionParameter);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                if (e.Message.Contains("Back-office is currently updating"))
                    throw new BackOfficeOfflineException("back office exception detected", e);
            }

            var taskResult =new TaskResult {IsSuccessful = true};
            taskResult.SetData(webDriver);
            return taskResult;
        }
    }
}