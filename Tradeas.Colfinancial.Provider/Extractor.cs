using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Tradeas.Colfinancial.Provider.Models;
using Tradeas.Colfinancial.Provider.Builders;
using Tradeas.Colfinancial.Provider.Processors;
using OpenQA.Selenium.Support.UI;
using System;
using log4net;
using Tradeas.Service.Models;

namespace Tradeas.Colfinancial.Provider
{
    public class Extractor : IExtractor
    {
        private readonly static ILog Logger = LogManager.GetLogger(typeof(IExtractor));
        private readonly IWebDriver _webDriver;
        private readonly IBuilder _colfinancialTransactionBuilder;
        private readonly IDatabaseProcessor _databaseProcessor;
        private const string LoginPage = "https://www.colfinancial.com/ape/Final2/home/HOME_NL_MAIN.asp?p=0";
        private const string HomePage = "https://ph10.colfinancial.com/ape/FINAL2_STARTER/HOME/HOME.asp";
        private const string User1TextboxName = "txtUser1";
        private const string User2TextboxName = "txtUser2";
        private const string PasswordTextboxName = "txtPassword";
        private const string SubmitButtonSelector = "input[value = 'LOG IN'][type = 'button']";
        private const string HeaderFrameId = "headern";
        private const string MainFrameId = "main";
        private const string TradeTabSelector = "a[onclick='ital(3);clickTable(3,1);getwin(3);'][onmouseover='displayTable(3);'][onmouseout='hideTable();']";
        private const string TradeHistorySelector = "a[onclick='ital(43);clickTable(3,2);getwin(43);'][onmouseover='displayTable(3);'][onmouseout='hideTable();']";
        private const string MonthlyRadioSelector = "input[name='rdCompTrade'][value='2']";
        private const string TradeHistorySubmitName = "cmdCompletedTrade";
        private const string TradeHistoryTableSelector = "table[class='reference']";
        private const string TableBodyTag = "tbody";
        private const string TableRowTag = "tr";
            
        public Extractor(IWebDriver webDriver,
                         IBuilder colfinancialTransactionBuilder,
                         IDatabaseProcessor databaseProcessor)
        {
            _webDriver = webDriver;
            _colfinancialTransactionBuilder = colfinancialTransactionBuilder;
            _databaseProcessor = databaseProcessor;
        }

        /// <summary>
        /// Scrap this instance.
        /// </summary>
        public async Task Extract(TransactionParameter transactionParameter)
        {
            //login
            try
            {
                var webDriverWait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
                _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

                Logger.Info($"navigating to login page {LoginPage}");
                _webDriver.Navigate().GoToUrl(LoginPage);
                _webDriver.Manage().Window.Maximize();

                var usernameTokens = transactionParameter
                    .LoginCredential
                    .Username
                    .Split('-');
                _webDriver.FindElement(By.Name(User1TextboxName)).SendKeys(usernameTokens[0]);
                _webDriver.FindElement(By.Name(User2TextboxName)).SendKeys(usernameTokens[1]);
                _webDriver.FindElement(By.Name(PasswordTextboxName)).SendKeys(transactionParameter.LoginCredential.Password);
                _webDriver.FindElement(By.CssSelector(SubmitButtonSelector)).Click();

                webDriverWait.Until(d => d.Url.ToLower() == HomePage.ToLower());

                //find iframe
                _webDriver.Navigate().GoToUrl(HomePage);
                Logger.Info($"navigating to home page {HomePage}");

                _webDriver.SwitchTo().Frame(_webDriver.FindElement(By.Id(HeaderFrameId)));
                _webDriver.FindElement(By.CssSelector(TradeTabSelector)).Click();
                Logger.Info($"trade tab click success");
                _webDriver.FindElement(By.CssSelector(TradeHistorySelector)).Click();
                Logger.Info($"trade history click success");

                _webDriver.SwitchTo().ParentFrame();
                _webDriver.SwitchTo().Frame(_webDriver.FindElement(By.Id(MainFrameId)));
                Logger.Info($"switching to main frame success");

                Thread.Sleep(TimeSpan.FromSeconds(3));
                webDriverWait.Until(d => d.FindElement(By.CssSelector(MonthlyRadioSelector)));
                //if (!isMonthlyHistoryValid) throw new Exception("monthly transaction history cannot be detected");

                _webDriver.FindElement(By.CssSelector(MonthlyRadioSelector)).Click();
                Logger.Info($"selecting monthly trade history");
                _webDriver.FindElement(By.Name(TradeHistorySubmitName)).Click();
                Logger.Info($"monthly trade history click success");

                Thread.Sleep(TimeSpan.FromSeconds(3));
                webDriverWait.Until(d => d.FindElement(By.CssSelector(TradeHistoryTableSelector)));
                // need to test for no trades
                var tables = _webDriver.FindElements(By.CssSelector(TradeHistoryTableSelector));
                foreach (var table in tables)
                {
                    var tbody = table.FindElement(By.TagName(TableBodyTag));
                    var rows = tbody.FindElements(By.TagName(TableRowTag));
                    var transactions = (List<Transaction>)_colfinancialTransactionBuilder.Build(rows);
                    await _databaseProcessor.BulkAsync(transactions);
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                _webDriver.Close();
                _webDriver.Dispose();
            }
        }
    }
}
