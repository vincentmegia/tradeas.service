using System.IO;
using System.Reflection;
using log4net;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Tradeas.Colfinancial.Provider.Actors;

namespace Tradeas.Colfinancial.Provider
{
    public static class WebDriverFactory
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerActor));
        
        public static IWebDriver Create(IConfiguration configuration)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("no-sandbox");
            var path = configuration["ChromeDriverPath"];
            Logger.Info($"setting chromedriver.exe path {path}");
            options.BinaryLocation = path;
            var chromeDriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new ChromeDriver(chromeDriverPath, options);
        }
    }
}