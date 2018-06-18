using System.Collections.Generic;
using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface IImportRepository
    {
        Task<Result> BulkAsync(List<string> imports);
        Task<Result> GetAll();
    }
}