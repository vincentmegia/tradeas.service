using System.Collections.Generic;
using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Processors
{
    public interface ITransactionProcessor
    {
        Task<Result> Process(Transaction transaction);
        Task<Result> Process(List<Transaction> transactions);
    }
}