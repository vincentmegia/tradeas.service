using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Tradeas.Colfinancial.Provider.Builders;
using Tradeas.Colfinancial.Provider.Processors;
using OpenQA.Selenium.Support.UI;
using System;
using log4net;
using Tradeas.Colfinancial.Provider.Scrapers;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider
{
    public class Extractor : IExtractor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IExtractor));
        private readonly IWebDriver _webDriver;
        private readonly ITransactionScraper _transactionScraper;
        private readonly IPortfolioScraper _portfolioScraper;

        private const string LoginPageUrl = "https://www.colfinancial.com/ape/Final2/home/HOME_NL_MAIN.asp?p=0";
        private const string HomePageLikeUrl = "ape/FINAL2_STARTER/HOME/HOME.asp";
        private const string User1TextboxName = "txtUser1";
        private const string User2TextboxName = "txtUser2";
        private const string PasswordTextboxName = "txtPassword";
        private const string SubmitButtonSelector = "input[value = 'LOG IN'][type = 'button']";
        
            
        public Extractor(IWebDriver webDriver,
                         ITransactionScraper transactionScraper,
                         IPortfolioScraper portfolioScraper)
        {
            _webDriver = webDriver;
            _transactionScraper = transactionScraper;
            _portfolioScraper = portfolioScraper;
        }

        /// <summary>
        /// Scrap this instance.
        /// </summary>
        public async Task<Result> Extract(TransactionParameter transactionParameter)
        {
            //login
            try
            {
                
                _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
                var loginResult = Login(transactionParameter);

                //find iframe
                _webDriver.Navigate().GoToUrl(_webDriver.Url);
                Logger.Info($"navigating to home page {HomePageLikeUrl}");

                var transactionScraperResult = await _transactionScraper.Scrape(_webDriver);
                var portfolioScraperResult = await _portfolioScraper.Scrape(_webDriver);

                return new TaskResult
                {
                    IsSuccessful = transactionScraperResult.IsSuccessful.Value &&
                                   portfolioScraperResult.IsSuccessful.Value
                };
            }
            catch(Exception e)
            {
                Logger.Error(e);
                throw;
            }
            finally
            {
                _webDriver.Close();
                _webDriver.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionParameter"></param>
        /// <returns></returns>
        private Result Login(TransactionParameter transactionParameter)
        {
            Logger.Info($"navigating to login page {LoginPageUrl}");
            _webDriver.Navigate().GoToUrl(LoginPageUrl);
            _webDriver.Manage().Window.Maximize();

            var usernameTokens = transactionParameter
                .LoginCredential
                .Username
                .Split('-');
            _webDriver.FindElement(By.Name(User1TextboxName)).SendKeys(usernameTokens[0]);
            _webDriver.FindElement(By.Name(User2TextboxName)).SendKeys(usernameTokens[1]);
            _webDriver.FindElement(By.Name(PasswordTextboxName)).SendKeys(transactionParameter.LoginCredential.Password);
            _webDriver.FindElement(By.CssSelector(SubmitButtonSelector)).Click();

            var webDriverWait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            webDriverWait.Until(d =>
            {
                Logger.Info($"checking if url {d.Url.ToLower()} contains {HomePageLikeUrl.ToLower()}");
                return d.Url.ToLower().Contains(HomePageLikeUrl.ToLower());
            });
            return new TaskResult {IsSuccessful = true};
        }
    }
}
