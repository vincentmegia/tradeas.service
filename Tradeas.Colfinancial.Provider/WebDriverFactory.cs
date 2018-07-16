using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Tradeas.Colfinancial.Provider
{
    public static class WebDriverFactory
    {
        public static IWebDriver Create()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("-no-sandbox");
            var chromeDriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new ChromeDriver(chromeDriverPath, options);
        }
    }
}