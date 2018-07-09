using log4net;
using OpenQA.Selenium;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Navigators
{
    public class LoginNavigator
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LoginNavigator));
        private readonly IWebDriver _webDriver;

        public LoginNavigator(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult Navigate()
        {
            //_webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            
            Logger.Info($"navigating to login page {Constants.LoginPageUrl}");
            _webDriver.Navigate().GoToUrl(Constants.LoginPageUrl);
            //_webDriver.Manage().Window.Maximize();
            
            _webDriver.Navigate().GoToUrl(_webDriver.Url);
            Logger.Info($"navigating to home page {Constants.HomePageLikeUrl}");
            return new TaskResult {IsSuccessful = true};
        }
    }
}