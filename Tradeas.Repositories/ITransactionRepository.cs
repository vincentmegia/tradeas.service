using System.Collections.Generic;
using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface ITransactionRepository
    {
        Task<Result> BulkAsync(List<string> transactions);
        Task<Result> PutAsync(Transaction transaction);
        Task<Result> GetOrphanTransactions();
    }
}