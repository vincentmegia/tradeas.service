using System.Threading.Tasks;
using OpenQA.Selenium;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Scrapers
{
    public interface IPortfolioScraper
    {
        Task<Result> Scrape(IWebDriver webDriver);
    }
}