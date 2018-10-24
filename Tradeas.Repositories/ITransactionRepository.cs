using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface ITransactionRepository : IRepository
    {
        Task<Result> PutAsync(Transaction transaction);
        Task<Result> GetOrphanTransactions();
    }
}