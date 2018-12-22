using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface IImportTrackerRepository : IRepository
    {
        TaskResult PutAsync(ImportTracker importTracker);
        TaskResult PostAsync(ImportTracker importTracker);
        TaskResult GetAll();
        TaskResult DeleteAsync(ImportTracker importTracker);
    }
}