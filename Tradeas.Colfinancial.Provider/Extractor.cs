﻿using System.Threading.Tasks;
using OpenQA.Selenium;
using System;
using log4net;
using Tradeas.Colfinancial.Provider.Scrapers;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider
{
    public class Extractor : IExtractor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Extractor));
        private readonly ITransactionScraper _transactionScraper;
        private readonly IPortfolioScraper _portfolioScraper;
        private readonly BrokerTransactionScraper _brokerTransactionScraper;

        
        
        
            
        public Extractor(ITransactionScraper transactionScraper,
                         IPortfolioScraper portfolioScraper,
                         BrokerTransactionScraper brokerTransactionScraper)
        {
            _transactionScraper = transactionScraper;
            _portfolioScraper = portfolioScraper;
            _brokerTransactionScraper = brokerTransactionScraper;
        }

        /// <summary>
        /// Scrap this instance.
        /// </summary>
        public async Task<Result> Extract(TransactionParameter transactionParameter)
        {
            Logger.Info("initiating extraction");
            //var transactionScraperResult = await _transactionScraper.Scrape(_webDriver);
            //var portfolioScraperResult = await _portfolioScraper.Scrape(_webDriver);
            var result = await _brokerTransactionScraper.Scrape(transactionParameter);
            
            try
            {
                return new TaskResult
                {
                    IsSuccessful = //transactionScraperResult.IsSuccessful.Value &&
                        //portfolioScraperResult.IsSuccessful.Value
                        //brokerTransactionScraper.IsSuccessful
                        true
                };
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
            finally
            {

            }

            return new TaskResult {IsSuccessful = true};
        }
    }
}
