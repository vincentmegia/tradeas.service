using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using Tradeas.Models;

namespace Tradeas.Service.Api.Builders
{
    public interface IJournalBuilder
    {
        List<Position> Positions { get; }
        JournalBuilder Build(ReadOnlyCollection<IWebElement> rows);
        JournalBuilder CreateStageIdeas();
    }
}