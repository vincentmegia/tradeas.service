using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Scrapers
{
    public class PortfolioScraper : IPortfolioScraper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TradeTransactionScraper));
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="webDriver"></param>
        /// <returns></returns>
        public async Task<Result> Scrape(IWebDriver webDriver)
        {
            webDriver.SwitchTo().Frame(webDriver.FindElement(By.Id(Constants.HeaderFrameId)));
            webDriver.FindElement(By.CssSelector(Constants.TradeTabSelector)).Click();
            Logger.Info($"trade tab click success");
            webDriver.FindElement(By.CssSelector(Constants.PortfolioTabSelector)).Click();
            Logger.Info($"portfolio tab click success");

            //var webDriverWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            Thread.Sleep(TimeSpan.FromSeconds(3));
            
            webDriver.SwitchTo().ParentFrame();
            webDriver.SwitchTo().Frame(webDriver.FindElement(By.Id(Constants.MainFrameId)));
            Logger.Info($"switching to main frame success");

            var accountEquity = webDriver.FindElement(By.XPath("/html/body/div[2]/table/tbody/tr/td/p[1]/font[2]/strong")).Text;
            var actualBalance = webDriver.FindElement(By.XPath("/html/body/div[2]/table/tbody/tr/td/table[1]/tbody/tr[3]/td[1]/font")).Text;
            var buyingPower = webDriver.FindElement(By.XPath("/html/body/div[2]/table/tbody/tr/td/table[1]/tbody/tr[3]/td[2]/font")).Text;
            var dayChangePercentage = webDriver.FindElement(By.XPath("/html/body/div[2]/table/tbody/tr/td/table[2]/tbody/tr[3]/td/table/tbody/tr[9]/td[2]/font/b")).Text;
            var dayChangeValue = webDriver.FindElement(By.XPath("/html/body/div[2]/table/tbody/tr/td/table[2]/tbody/tr[3]/td/table/tbody/tr[9]/td[3]/font/b")).Text;
            var portfolioGainLossPercentage = webDriver.FindElement(By.XPath("/html/body/div[2]/table/tbody/tr/td/table[2]/tbody/tr[3]/td/table/tbody/tr[10]/td[2]/font/b")).Text;
            var portfolioGainLossValue = webDriver.FindElement(By.XPath("/html/body/div[2]/table/tbody/tr/td/table[2]/tbody/tr[3]/td/table/tbody/tr[10]/td[3]/font/b")).Text;
            
            Logger.Info($"account equity: {accountEquity}");
            Logger.Info($"actual balance: {actualBalance}");
            Logger.Info($"buying power: {buyingPower}");
            Logger.Info($"day change%: {dayChangePercentage}");
            Logger.Info($"day change: {dayChangeValue}");
            Logger.Info($"portfolio gain/loss%: {portfolioGainLossPercentage}");
            Logger.Info($"portfolio gain/loss: {portfolioGainLossValue}");

            var portfolioSnapshot = new PortfolioSnapshot
            {
                TotalEquity= Convert.ToDecimal(accountEquity),
                Balance= Convert.ToDecimal(actualBalance),
                BuyingPower = Convert.ToDecimal(buyingPower),
                GainLossValue = Convert.ToDecimal(portfolioGainLossValue),
                GainLossPercentage = Convert.ToDecimal(portfolioGainLossPercentage),
                DayChangePercentage = Convert.ToDecimal(dayChangePercentage),
                DayChangeValue = Convert.ToDecimal(dayChangeValue),
                BrokerCode = "Col",
                CreatedDate = new DateTime?()
            };
            Logger.Info($"monthly trade history click success");
            var taskResult = new TaskResult {IsSuccessful = true}.SetData(portfolioSnapshot);
            return taskResult;
        }
    }
}