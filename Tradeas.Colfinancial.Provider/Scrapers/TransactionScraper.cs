using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Tradeas.Colfinancial.Provider.Builders;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Scrapers
{
    public class TransactionScraper : ITransactionScraper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransactionScraper));
        private const string TradeHistorySelector = "a[onclick='ital(43);clickTable(3,2);getwin(43);'][onmouseover='displayTable(3);'][onmouseout='hideTable();']";
        private const string MonthlyRadioSelector = "input[name='rdCompTrade'][value='2']";
        private const string TradeHistorySubmitName = "cmdCompletedTrade";
        private const string TradeHistoryTableSelector = "table[class='reference']";
        private const string TableBodyTag = "tbody";
        private const string TableRowTag = "tr";
        private readonly ITransactionBuilder _transactionBuilder;
        private readonly ITransactionProcessor _transactionProcessor;

        public TransactionScraper(ITransactionBuilder transactionBuilder,
                                  ITransactionProcessor transactionProcessor)
        {
            _transactionBuilder = transactionBuilder;
            _transactionProcessor = transactionProcessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Result> Scrape(IWebDriver webDriver)
        {
            webDriver.SwitchTo().Frame(webDriver.FindElement(By.Id(Constants.HeaderFrameId)));
            webDriver.FindElement(By.CssSelector(Constants.TradeTabSelector)).Click();
            Logger.Info($"trade tab click success");
            webDriver.FindElement(By.CssSelector(TradeHistorySelector)).Click();
            Logger.Info($"trade history click success");

            webDriver.SwitchTo().ParentFrame();
            webDriver.SwitchTo().Frame(webDriver.FindElement(By.Id(Constants.MainFrameId)));
            Logger.Info($"switching to main frame success");

            var webDriverWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            Thread.Sleep(TimeSpan.FromSeconds(3));
            webDriverWait.Until(d => d.FindElement(By.CssSelector(MonthlyRadioSelector)));
            //if (!isMonthlyHistoryValid) throw new Exception("monthly transaction history cannot be detected");

            webDriver.FindElement(By.CssSelector(MonthlyRadioSelector)).Click();
            Logger.Info($"selecting monthly trade history");
            Thread.Sleep(TimeSpan.FromSeconds(3));
            webDriver.FindElement(By.Name(TradeHistorySubmitName)).Click();
            Logger.Info($"monthly trade history click success");

            Thread.Sleep(TimeSpan.FromSeconds(10));
            webDriverWait.Until(d => d.FindElement(By.CssSelector(TradeHistoryTableSelector)));
            // need to test for no trades
            var tables = webDriver.FindElements(By.CssSelector(TradeHistoryTableSelector));
            foreach (var table in tables)
            {
                var tbody = table.FindElement(By.TagName(TableBodyTag));
                var rows = tbody.FindElements(By.TagName(TableRowTag));

                var transactions = (List<Transaction>) _transactionBuilder.Build(rows);
                var transactionResult = await _transactionProcessor.Process(transactions);
            }

            return new TaskResult {IsSuccessful = true};
        }
    }
}