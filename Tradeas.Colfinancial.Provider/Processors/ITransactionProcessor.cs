using System.Collections.Generic;
using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Processors
{
    public interface ITransactionProcessor
    {
        Task<Result<Transaction>> Process(Transaction transaction);
        Task<Result<List<Transaction>>> Process(List<Transaction> transactions);
    }
}