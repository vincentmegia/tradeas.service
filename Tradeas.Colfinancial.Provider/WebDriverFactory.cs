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
    public class WebDriverFactory
    {
        private readonly ILog Logger = LogManager.GetLogger(typeof(BatchActor));
        private readonly IConfiguration _configuration;

        public WebDriverFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IWebDriver Create()
        {
            var options = new ChromeOptions();
            options.AddArgument("headless");
            options.AddArgument("no-sandbox");
            switch (Environment.OSVersion.Platform)
            {
                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.WinCE:
                        options.BinaryLocation = _configuration["ChromeDriver:Windows"];
                        break;
            }
                
            Logger.Info($"setting chromedriver.exe path {options.BinaryLocation}");
            var chromeDriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new ChromeDriver(chromeDriverPath, options);
        }
    }
}