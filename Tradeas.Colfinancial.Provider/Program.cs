using OpenQA.Selenium.Chrome;
using Tradeas.Colfinancial.Provider.Builders;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Colfinancial.Provider.Repositories;
using MyCouch;
using System.Threading.Tasks;

namespace Tradeas.Colfinancial.Provider
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            
            Task.Run(async () =>
            {
                ChromeOptions options = new ChromeOptions();
                //options.AddArgument("--headless");

                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.SuppressInitialDiagnosticInformation = true;
                service.HideCommandPromptWindow = true;

                var colfinancialScraper = new Extractor(new ChromeDriver(options),
                                    new TransactionBuilder(),
                                     new DatabaseProcessor(new TransactionRepository(new MyCouchClient("http://127.0.0.1:5984", "transactions"))));
                await colfinancialScraper.Extract();
            }).GetAwaiter().GetResult();
        }
    }
}