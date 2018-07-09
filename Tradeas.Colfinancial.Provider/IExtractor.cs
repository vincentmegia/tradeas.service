using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider
{
    public interface IExtractor
    {
        TaskResult Extract(TransactionParameter transactionParameter);
    }
}