using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Repository;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Tradeas.Colfinancial.Provider.Builders;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Models;
using Tradeas.Repositories;

namespace Tradeas.Colfinancial.Provider.Scrapers
{
    public class BrokerTransactionScraper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TradeTransactionScraper));
        private const string QuoteTabSelector = "a[onclick='ital(2);clickTable(2,1);getwin(2);'][onmouseover='displayTable(2);'][onmouseout='hideTable();']";
        private const string BrokerSubTabSelector = "a[onclick='ital(24);clickSubTable1(3);getwin(24);'][onmouseover='delaySubTable1(3);'][onmouseout='hideSubTable1();']";
        private const string InputId = "ebrokerno";
        private const string DateFromSelectName = "cbDateFrom";
        private readonly BrokerTransactionProcessor _brokerTransactionProcessor;
        private readonly BrokerTransactionBuilder _brokerTransactionBuilder;
        private readonly IImportRepository _importRepository; //consider if repository layer is correct in this layer

        public BrokerTransactionScraper(BrokerTransactionProcessor brokerTransactionProcessor,
                                        BrokerTransactionBuilder brokerTransactionBuilder,
                                        IImportRepository importRepository)
        {
            _brokerTransactionProcessor = brokerTransactionProcessor;
            _brokerTransactionBuilder = brokerTransactionBuilder;
            _importRepository = importRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public async Task<Result> Scrape(IWebDriver webDriver)
        {
            var importsResponse = await _importRepository.GetAll();
            var imports = importsResponse.GetData<List<Import>>();

            SelectBrokerTab(webDriver);
            SelectBrokerTransactionPane(webDriver);
            
            foreach (var import in imports)
            {
                Logger.Info($"processing symbol: {import.Symbol}");
                var exportedList = new List<string>
                {
                    "2GO", "ACE","FGENF","FGENG","FJP","FJPB","FLI","FMETF","FOOD","FPH","FPI","GEO","GERI","GLO","GMA7","GMAP","GPH","GSMI","GTCAP","H2O",
                    "HI","HLCM","HOUSE","I","IMP","ION","IPO","IRC","IMP","ION","IPO","IRC","IS","ISM","JFC","JGS","JOH","KEP","KPH","KPHB","LAND","LC","LCB",
                    "LFM","LMG","LOTO","LPZ","LR","LRP","LRW","LSC","LTG","MA","MAB","MAC","MACAY","MAH","MARC","MB","MBC","MBT","MED","MEG","MER","MFC","MFIN",
                    "MG","MHC","MJC","MJIC","MRC","MVC","MWC","MWIDE",",NI","NIKL","NOW","NRCP","OM","OPM","ORE","OV","PA","PAL","PBB","PBC","PCOR","PERC"
                    
                };
                
                if (exportedList.Contains(import.Symbol))
                {
                    Logger.Info($"symbol {import.Symbol} has already been processed. skipping");
                    continue;
                }
                
                var input = webDriver.FindElement(By.Id(InputId));
                input.Clear(); 
                input.SendKeys(import.Symbol);
                
                Logger.Info($"setting stock name: {import.Symbol} success");

                var today = DateTime.Now.AddMonths(-2).ToString("yyyy/MM/dd");
                var javaScriptExecutor = (IJavaScriptExecutor)webDriver;
                var script = $"var dateFromDropdown=document.getElementsByName('cbDateFrom')[0];var option=document.createElement('option');option.text='{today}';option.value='{today}';dateFromDropdown.add(option);";
                javaScriptExecutor.ExecuteScript(script);
            
                var dateFromSelect = new SelectElement(webDriver.FindElement(By.Name(DateFromSelectName)));
                dateFromSelect.SelectByValue(today);
                Logger.Info($"setting date from value success");

                webDriver.FindElement(By.Id("bsubmit")).Click();
                Logger.Info($"setting date to value success");
            
                webDriver.SwitchTo().ParentFrame();
                webDriver.SwitchTo().Frame(webDriver.FindElement(By.Name("brokertrxnout2")));
            
                Logger.Info("sleeping for 2mins to wait result of broker transaction query.");
                Thread.Sleep(TimeSpan.FromMinutes(2));
                Logger.Info("awoken from 2 min sleep.");

                
                try
                {
                    var table = webDriver.FindElement(By.XPath("/html/body/form/table"));
                    var tbody = table.FindElement(By.TagName("tbody"));
                    var rows = tbody.FindElements(By.TagName("tr"));
                    if (rows.Count > 0)
                    {
                        var brokerTransactions = _brokerTransactionBuilder.Build(rows, import.Symbol);
                        await _brokerTransactionProcessor.Process(brokerTransactions);   
                        _brokerTransactionBuilder.Transactions.Clear();
                    }
                    Logger.Info($"broker transactions for completed for symbol: {import.Symbol}");  
                }
                catch (Exception e)
                {
                    Logger.Warn($"No broker transactions found for symbol {import.Symbol}", e);
                } 
                
                //switch back to main frame to prevent
                webDriver.SwitchTo().ParentFrame();
                webDriver.SwitchTo().ParentFrame();
                SelectBrokerTab(webDriver);
                SelectBrokerTransactionPane(webDriver);
            }
            

            return new TaskResult {IsSuccessful = true};
        }

        /// <summary>
        /// 
        /// </summary>
        private void SelectBrokerTab(IWebDriver webDriver)
        {
            webDriver.SwitchTo().Frame(webDriver.FindElement(By.Id(Constants.HeaderFrameId)));
            webDriver.FindElement(By.CssSelector(QuoteTabSelector)).Click();
            Logger.Info($"quote tab click success");
                
            webDriver.FindElement(By.CssSelector(BrokerSubTabSelector)).Click();
            Logger.Info($"broker information sub tab click success");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webDriver"></param>
        private void SelectBrokerTransactionPane(IWebDriver webDriver)
        {
            Thread.Sleep(TimeSpan.FromSeconds(3));
            webDriver.SwitchTo().ParentFrame();
            webDriver.SwitchTo().Frame(webDriver.FindElement(By.Name(Constants.MainFrameId)));
            webDriver.SwitchTo().Frame(webDriver.FindElement(By.Name("brokertrxnin2")));
            Logger.Info($"switching to main frame success");           
        }
    }
}