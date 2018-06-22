using System;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Simulators
{
    public class BrokerTransactionSimulator
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerTransactionSimulator));
        private readonly IWebDriver _webDriver;
        
        public BrokerTransactionSimulator(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Result> Simulate(string symbol)
        {
            var input = _webDriver.FindElement(By.Id(Constants.InputId));
            input.Clear();
            input.SendKeys(symbol);

            Logger.Info($"setting stock name: {symbol} success");

            var today = DateTime.Now.AddMonths(-2).ToString("yyyy/MM/dd");
            var javaScriptExecutor = (IJavaScriptExecutor) _webDriver;
            var script =
                $"var dateFromDropdown=document.getElementsByName('cbDateFrom')[0];var option=document.createElement('option');option.text='{today}';option.value='{today}';dateFromDropdown.add(option);";
            javaScriptExecutor.ExecuteScript(script);

            var dateFromSelect = new SelectElement(_webDriver.FindElement(By.Name(Constants.DateFromSelectName)));
            dateFromSelect.SelectByValue(today);
            Logger.Info($"setting date from value success");

            _webDriver.FindElement(By.Id("bsubmit")).Click();
            Logger.Info($"setting date to value success");

            _webDriver.SwitchTo().ParentFrame();
            _webDriver.SwitchTo().Frame(_webDriver.FindElement(By.Name("brokertrxnout2")));

            Logger.Info("sleeping for 1mins to wait result of broker transaction query.");
            Thread.Sleep(TimeSpan.FromMinutes(1));
            Logger.Info("awoken from sleep.");

            return new TaskResult {IsSuccessful = true};
        }
    }
}