using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tradeas.Repositories
{
    public interface IBrokerTransactionRepository
    {
        Task BulkAsync(List<string> brokerTransactions);
    }
}