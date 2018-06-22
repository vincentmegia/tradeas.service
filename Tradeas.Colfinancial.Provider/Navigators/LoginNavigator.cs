using System.Threading.Tasks;
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
        /// <param name="transactionParameter"></param>
        /// <returns></returns>
        public async Task<Result> Navigate()
        {
            Logger.Info($"navigating to login page {Constants.LoginPageUrl}");
            _webDriver.Navigate().GoToUrl(Constants.LoginPageUrl);
            _webDriver.Manage().Window.Maximize();

            return new TaskResult {IsSuccessful = true};          
        }
    }
}