﻿using System;
using log4net;
using Tradeas.Colfinancial.Provider.Actors;
using Tradeas.Colfinancial.Provider.Scrapers;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider
{
    public class Extractor : IExtractor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Extractor));
        private readonly ITransactionScraper _transactionScraper;
        private readonly IPortfolioScraper _portfolioScraper;
        private readonly DirectorActor _directorActor;
        
            
        public Extractor(ITransactionScraper transactionScraper,
                         IPortfolioScraper portfolioScraper,
                         DirectorActor directorActor)
        {
            _transactionScraper = transactionScraper;
            _portfolioScraper = portfolioScraper;
            _directorActor = directorActor;
        }

        /// <summary>
        /// Scrap this instance.
        /// </summary>
        public TaskResult Extract(TransactionParameter transactionParameter)
        {
            Logger.Info("initiating extraction");
            //var transactionScraperResult = await _transactionScraper.Scrape(_webDriver);
            //var portfolioScraperResult = await _portfolioScraper.Scrape(_webDriver);
            try
            {
                var result = _directorActor.Do(transactionParameter);
                return result;
                /*{
                    IsSuccessful = //transactionScraperResult.IsSuccessful.Value &&
                        //portfolioScraperResult.IsSuccessful.Value
                        //brokerTransactionScraper.IsSuccessful
                        true
                };*/
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.Info("task completed");
            return new TaskResult {IsSuccessful = true};
        }
    }
}
