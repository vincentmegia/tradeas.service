using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Tradeas.Colfinancial.Provider.Builders;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Scrapers
{
    public class BrokerTransactionScraper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TradeTransactionScraper));
        private const string QuoteTabSelector = "a[onclick='ital(2);clickTable(2,1);getwin(2);'][onmouseover='displayTable(2);'][onmouseout='hideTable();']";
        private const string BrokerSubTabSelector = "a[onclick='ital(24);clickSubTable1(3);getwin(24);'][onmouseover='delaySubTable1(3);'][onmouseout='hideSubTable1();']";
        private const string InputId = "ebrokerno";
        private const string DateFromSelectName = "cbDateFrom";
        private BrokerTransactionProcessor _brokerTransactionProcessor;
        private BrokerTransactionBuilder _brokerTransactionBuilder;

        public BrokerTransactionScraper(BrokerTransactionProcessor brokerTransactionProcessor,
                                        BrokerTransactionBuilder brokerTransactionBuilder)
        {
            _brokerTransactionProcessor = brokerTransactionProcessor;
            _brokerTransactionBuilder = brokerTransactionBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public async Task<Result> Scrape(IWebDriver webDriver, string symbol)
        {
            webDriver.SwitchTo().Frame(webDriver.FindElement(By.Id(Constants.HeaderFrameId)));
            webDriver.FindElement(By.CssSelector(QuoteTabSelector)).Click();
            Logger.Info($"quote tab click success");
            
            webDriver.FindElement(By.CssSelector(BrokerSubTabSelector)).Click();
            Logger.Info($"broker information sub tab click success");

            Thread.Sleep(TimeSpan.FromSeconds(3));
            
            webDriver.SwitchTo().ParentFrame();
            webDriver.SwitchTo().Frame(webDriver.FindElement(By.Name(Constants.MainFrameId)));
            webDriver.SwitchTo().Frame(webDriver.FindElement(By.Name("brokertrxnin2")));
            Logger.Info($"switching to main frame success");
            
            webDriver.FindElement(By.Id(InputId)).SendKeys("VITA");
            Logger.Info($"setting stock name success");

            var javaScriptExecutor = (IJavaScriptExecutor)webDriver;
            var script = "var dateFromDropdown=document.getElementsByName('cbDateFrom')[0];var option=document.createElement('option');option.text='2018/04/10';option.value='2018/04/14';dateFromDropdown.add(option);";
            javaScriptExecutor.ExecuteScript(script);
            
            var dateFromSelect = new SelectElement(webDriver.FindElement(By.Name(DateFromSelectName)));
            dateFromSelect.SelectByValue("2018/04/14");
            Logger.Info($"setting date from value success");

            webDriver.FindElement(By.Id("bsubmit")).Click();
            Logger.Info($"setting date to value success");
            
            webDriver.SwitchTo().ParentFrame();
            webDriver.SwitchTo().Frame(webDriver.FindElement(By.Name("brokertrxnout2")));
            
            Thread.Sleep(TimeSpan.FromMinutes(2));
            var webDriverWait = new WebDriverWait(webDriver, TimeSpan.FromMinutes(2));
            webDriverWait.Until(d => d.FindElement(By.XPath("/html/body/form/table")));

            var table = webDriver.FindElement(By.XPath("/html/body/form/table"));
            var tbody = table.FindElement(By.TagName("tbody"));
            var rows = tbody.FindElements(By.TagName("tr"));
            var brokerTransactions = _brokerTransactionBuilder.Build(rows, symbol);
            await _brokerTransactionProcessor.Process(brokerTransactions);

            return new TaskResult {IsSuccessful = true};
        }
    }
}