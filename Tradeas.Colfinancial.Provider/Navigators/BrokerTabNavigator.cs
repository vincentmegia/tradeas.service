using System;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Navigators
{
    public class BrokerTabNavigator
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerTabNavigator));
        private readonly IWebDriver _webDriver;
        
        public BrokerTabNavigator(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionParameter"></param>
        /// <returns></returns>
        public async Task<Result> Navigate()
        {
            _webDriver.SwitchTo().Frame(_webDriver.FindElement(By.Id(Constants.HeaderFrameId)));
            _webDriver.FindElement(By.CssSelector(Constants.QuoteTabSelector)).Click();
            Logger.Info($"quote tab click success");
                
            _webDriver.FindElement(By.CssSelector(Constants.BrokerSubTabSelector)).Click();
            Logger.Info($"broker information sub tab click success");
            
            Thread.Sleep(TimeSpan.FromSeconds(3));
            _webDriver.SwitchTo().ParentFrame();
            _webDriver.SwitchTo().Frame(_webDriver.FindElement(By.Name(Constants.MainFrameId)));
            _webDriver.SwitchTo().Frame(_webDriver.FindElement(By.Name("brokertrxnin2")));
            Logger.Info($"switching to main frame success");
            
            return new TaskResult {IsSuccessful = true};          
        }
    }
}