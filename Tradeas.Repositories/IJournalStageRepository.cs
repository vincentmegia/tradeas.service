using System.Collections.Generic;
using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface IJournalStageRepository
    {
        Task<Result> BulkAsync(List<string> ideasJson);
    }
}