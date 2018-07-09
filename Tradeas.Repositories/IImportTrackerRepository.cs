using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface IImportTrackerRepository : IRepository
    {
        TaskResult PostAsync(ImportTracker importTracker);
        TaskResult GetAll();
        TaskResult DeleteAsync(ImportTracker importTracker);
    }
}