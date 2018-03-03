using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider
{
    public interface IExtractor
    {
        Task<Result> Extract(TransactionParameter transactionParameter);
    }
}