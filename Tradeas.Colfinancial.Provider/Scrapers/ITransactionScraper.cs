using System.Threading.Tasks;
using OpenQA.Selenium;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Scrapers
{
    public interface ITransactionScraper
    {
        Task<Result> Scrape(IWebDriver webDriver);
    }
}