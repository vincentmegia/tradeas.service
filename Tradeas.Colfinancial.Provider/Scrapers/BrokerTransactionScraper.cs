using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;
using Tradeas.Colfinancial.Provider.Navigators;
using Tradeas.Colfinancial.Provider.Simulators;
using Tradeas.Models;
using Tradeas.Repositories;

namespace Tradeas.Colfinancial.Provider.Scrapers
{
    public class BrokerTransactionScraper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerTransactionScraper));
        private readonly IImportRepository _importRepository; //consider if repository layer is correct in this layer
        private readonly LoginNavigator _loginNavigator;
        private readonly BrokerTabNavigator _brokerTabNavigator;
        private readonly LoginSimulator _loginSimulator;
        private readonly BrokerTableScraper _brokerTableScraper;
        

        public BrokerTransactionScraper(LoginNavigator loginNavigator,
                                        BrokerTabNavigator brokerTabNavigator,
                                        LoginSimulator loginSimulator,
                                        BrokerTableScraper brokerTableScraper,
                                        IImportRepository importRepository)
        {
            _importRepository = importRepository;
            _loginNavigator = loginNavigator;
            _brokerTabNavigator = brokerTabNavigator;
            _loginSimulator = loginSimulator;
            _brokerTableScraper = brokerTableScraper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Result> Scrape()
        {
            var importsResponse = await _importRepository.GetAll();
            var imports = importsResponse
                .GetData<List<Import>>()
                .OrderBy(i => i.Symbol);

            var webDrivers = new List<IWebDriver>();
            var batchSize = imports.Count() / 10;
            for (var index = 0; index >= 9; index++)
            {
                var webDriver = WebDriverFactory.Create();
                if (index == 0)
                {
                    var loginNavigateResult = await _loginNavigator.Navigate();
                    var loginSimulateResult = await _loginSimulator.Simulate();
                }
                
                var batch = imports.Take(batchSize);
                var tabNavigatorResult = await _brokerTabNavigator.Navigate();
                var tableScraper = await _brokerTableScraper.Scrape();
                
                webDrivers.Add(webDriver);
                imports.Skip(batchSize);
            }

            await _brokerTabNavigator.Navigate();
            
            
                
                //switch back to main frame to prevent
                //webDriver.SwitchTo().ParentFrame();
                //webDriver.SwitchTo().ParentFrame();
                //webDriver.SwitchTo().Frame(webDriver.FindElement(By.Id(Constants.HeaderFrameId)));
                //webDriver.FindElement(By.CssSelector(BrokerSubTabSelector)).Click();
                //Logger.Info($"broker information sub tab click success");
                //SelectBrokerTransactionPane(webDriver);
                //webDriver.SwitchTo().ParentFrame();
                //SelectBrokerTab(webDriver);
            return new TaskResult {IsSuccessful = true};
        }
    }
}