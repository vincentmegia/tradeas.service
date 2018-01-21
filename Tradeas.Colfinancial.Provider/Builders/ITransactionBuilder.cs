using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Builders
{
    public interface ITransactionBuilder
    {
        List<Transaction> Transactions { get; }
        TransactionBuilder Build(ReadOnlyCollection<IWebElement> rows);
    }
}
