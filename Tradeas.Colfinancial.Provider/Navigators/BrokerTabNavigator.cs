using System;
using System.Threading;
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
        /// <returns></returns>
        public TaskResult Navigate()
        {
            return Navigate(false);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult Navigate(bool byPassQuoteTab)
        {
            _webDriver.SwitchTo().Frame(_webDriver.FindElement(By.Id(Constants.HeaderFrameId)));
            if (!byPassQuoteTab) _webDriver.FindElement(By.CssSelector(Constants.QuoteTabSelector)).Click();
            Logger.Info($"quote tab click success");
                
            _webDriver.FindElement(By.CssSelector(Constants.BrokerSubTabSelector)).Click();
            Logger.Info($"broker information sub tab click success");
            
            Thread.Sleep(TimeSpan.FromSeconds(3));
            _webDriver.SwitchTo().ParentFrame();
            _webDriver.SwitchTo().Frame(_webDriver.FindElement(By.Name(Constants.MainFrameId)));
            Logger.Info($"switching to main frame success");
            _webDriver.SwitchTo().Frame(_webDriver.FindElement(By.Name("brokertrxnin2")));

            return new TaskResult {IsSuccessful = true};
        }

        /// <summary>
        /// Returns to 
        /// </summary>
        /// <returns></returns>
        public TaskResult NavigateHeaderFrame()
        {
            _webDriver.SwitchTo().ParentFrame();
            _webDriver.SwitchTo().ParentFrame();
            return new TaskResult {IsSuccessful = true};
        }
    }
}