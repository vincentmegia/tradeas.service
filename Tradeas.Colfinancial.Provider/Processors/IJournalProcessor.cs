using System.Collections.Generic;
using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Processors
{
    public interface IJournalProcessor
    {
        Task UpdateIdeas(List<Idea> transactions);
    }
}