using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface IImportTrackerRepository
    {
        Task<Result> PostAsync(ImportTracker importTracker);
        Task<Result> GetAll();
    }
}