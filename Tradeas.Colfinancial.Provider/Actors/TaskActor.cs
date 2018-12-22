using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;
using Tradeas.Colfinancial.Provider.Scrapers;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Actors
{
    public class TaskActor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TaskActor));

        private readonly BrokerTransactionScraper _brokerTransactionScraper;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private IWebDriver WebDriverInstance { get; set; }
        public Task TaskInstance { get; set; }


        public TaskActor(WebDriverFactory webDriverFactory,
                         BrokerTransactionScraper brokerTransactionScraper,
                         CancellationTokenSource cancellationTokenSource)
        {
            WebDriverInstance = webDriverFactory.Create();
            _brokerTransactionScraper = brokerTransactionScraper;
            _cancellationTokenSource = cancellationTokenSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult Do(List<Import> batch, TransactionParameter transactionParameter)
        {
            TaskInstance = Task
                .Factory
                .StartNew(() => _brokerTransactionScraper.Scrape(transactionParameter, batch, WebDriverInstance),
                    _cancellationTokenSource.Token);
            var symbols = string.Join(",", batch.Select(b => b.Symbol));
            Logger.Info($"Thread id: { TaskInstance.Id } will processing the following symbols: { symbols }");
            LogicalThreadContext.Properties["thread-id"] = TaskInstance.Id;

            return new TaskResult { IsSuccessful = true };
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Logger.Info($"Disposing webdriver handle: ${WebDriverInstance.CurrentWindowHandle}");
            WebDriverInstance.Quit();
            WebDriverInstance.Dispose();
            
            Logger.Info($"Disposing task id: : ${TaskInstance.Id}");
            TaskInstance.Dispose();
        }
    }
}