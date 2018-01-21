using System.Collections.Generic;
using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface ITransactionRepository
    {
        Task BulkAsync(List<string> transactions);
        Task<ResponseResult<Transaction>> PutAsync(Transaction transaction);
    }
}