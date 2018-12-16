using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Tradeas.Colfinancial.Provider.Scrapers;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Actors
{
    public class TaskActor
    {
        private readonly WebDriverFactory _webDriverFactory;
        private readonly BrokerTransactionScraper _brokerTransactionScraper;
        private readonly TransactionParameter _transactionParameter;
        private CancellationToken _cancellationToken;
        
        public CancellationTokenSource CancellationTokenSourceInstance { get; set; }
        public IWebDriver WebDriverInstance { get; set; }
        public Task TaskInstance { get; set; }


        public TaskActor(WebDriverFactory webDriverFactory,
            BrokerTransactionScraper brokerTransactionScraper,
            TransactionParameter transactionParameter)
        {
            _webDriverFactory = webDriverFactory;
            _brokerTransactionScraper = brokerTransactionScraper;
            _transactionParameter = transactionParameter;
            CancellationTokenSourceInstance = new CancellationTokenSource();
            _cancellationToken = CancellationTokenSourceInstance.Token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult Do(List<Import> batch)
        {
            WebDriverInstance = _webDriverFactory.Create();
            TaskInstance = Task
                .Factory
                .StartNew(() => _brokerTransactionScraper.Scrape(_transactionParameter, batch, WebDriverInstance),
                    _cancellationToken);
            return new TaskResult();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            WebDriverInstance.Quit();
            WebDriverInstance.Dispose();
            TaskInstance.Dispose();
        }
    }
}