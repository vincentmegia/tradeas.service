using System.Collections.Generic;
using System.Threading.Tasks;
using Tradeas.Colfinancial.Provider.Models;

namespace Tradeas.Colfinancial.Provider.Processors
{
    public interface IDatabaseProcessor
    {
        Task BulkAsync(List<Transaction> transactions);
    }
}