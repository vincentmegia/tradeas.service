using System.Threading.Tasks;
using Tradeas.Service.Models;

namespace Tradeas.Colfinancial.Provider
{
    public interface IExtractor
    {
        Task Extract(TransactionParameter transactionParameter);
    }
}