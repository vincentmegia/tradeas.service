using System;
using System.IO;
using System.Reflection;
using log4net;
using Microsoft.Extensions.Configuration;
using MoreLinq;
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
            //options.AddArgument("headless");
            options.AddArgument("no-sandbox");
            switch (Environment.OSVersion.Platform)
            {
                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.WinCE:
                        options.BinaryLocation = configuration["ChromeDriver:Windows"];
                        break;
            }
                
            Logger.Info($"setting chromedriver.exe path {options.BinaryLocation}");
            var chromeDriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new ChromeDriver(chromeDriverPath, options);
        }
    }
}