using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface IImportRepository
    {
        Task<Result> GetAll();
    }
}