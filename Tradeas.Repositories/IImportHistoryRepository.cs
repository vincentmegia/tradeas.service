using System.Collections.Generic;
using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface IImportHistoryRepository
    {
        Task<Result> BulkAsync(List<string> brokerTransactions);
    }
}