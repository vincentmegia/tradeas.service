using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Tradeas.Colfinancial.Provider
{
    public static class WebDriverFactory
    {
        public static IWebDriver Create(IConfiguration configuration)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("-no-sandbox");
            options.BinaryLocation = configuration["ChromeDriverPath"];
            var chromeDriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new ChromeDriver(chromeDriverPath, options);
        }
    }
}