using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using Tradeas.Colfinancial.Provider.Models;

namespace Tradeas.Colfinancial.Provider.Builders
{
    public interface IBuilder
    {
        List<Transaction> Transactions { get; }
        TransactionBuilder Build(ReadOnlyCollection<IWebElement> rows);
    }
}
