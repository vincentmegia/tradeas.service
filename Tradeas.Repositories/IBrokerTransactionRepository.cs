using System.Collections.Generic;
using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface IBrokerTransactionRepository
    {
        Task<Result> BulkAsync(List<string> brokerTransactions);
    }
}