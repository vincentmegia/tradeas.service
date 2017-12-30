using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tradeas.Colfinancial.Provider.Repositories
{
    public interface ITransactionRepository
    {
        Task BulkAsync(List<string> transactions);
    }
}