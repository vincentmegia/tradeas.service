using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;
using Tradeas.Colfinancial.Provider.Builders;
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

        public BrokerTableScraper(List<Import> imports,
                                  BrokerTransactionBuilder brokerTransactionBuilder,
                                  BrokerTransactionProcessor brokerTransactionProcessor,
                                  BrokerTransactionSimulator brokerTransactionSimulator,
                                  IWebDriver webDriver,
                                  IImportTrackerRepository importTrackerRepository)
        {
            _webDriver = webDriver;
            _imports = imports;
            _importTrackerRepository = importTrackerRepository;
            _brokerTransactionBuilder = brokerTransactionBuilder;
            _brokerTransactionProcessor = brokerTransactionProcessor;
            _brokerTransactionSimulator = brokerTransactionSimulator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Result> Scrape()
        {
            var importTrackerResponse = await _importTrackerRepository.GetAll();
            var exportedList = importTrackerResponse.GetData<List<ImportTracker>>();

            foreach (var import in _imports)
            {
                Logger.Info($"processing symbol: {import.Symbol}");
                if (exportedList.Contains(new ImportTracker {Symbol = import.Symbol}))
                {
                    Logger.Info($"symbol {import.Symbol} has already been processed. skipping");
                    continue;
                }

                var transactionSimulatorResult = await _brokerTransactionSimulator.Simulate(import.Symbol);

                try
                {
                    var table = _webDriver.FindElement(By.XPath("/html/body/form/table"));
                    var tbody = table.FindElement(By.TagName("tbody"));
                    var rows = tbody.FindElements(By.TagName("tr"));
                    if (rows.Count > 0)
                    {
                        var brokerTransactions = _brokerTransactionBuilder.Build(rows, import.Symbol);
                        await _brokerTransactionProcessor.Process(brokerTransactions);
                        _brokerTransactionBuilder.Transactions.Clear();
                        var importTracker = new ImportTracker
                        {
                            Id = $"{import.Symbol}-{DateTime.Now:yyyyMMMyy}",
                            Symbol = import.Symbol
                        };
                        await _importTrackerRepository.PostAsync(importTracker);
                        exportedList.Add(importTracker);
                    }

                    Logger.Info($"broker transactions for completed for symbol: {import.Symbol}");
                }
                catch (Exception e)
                {
                    Logger.Warn($"No broker transactions found for symbol {import.Symbol}", e);
                }
            }
            
            return new TaskResult {IsSuccessful = true};
        }
    }
}