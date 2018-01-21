using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tradeas.Repositories
{
    public interface IJournalStageRepository
    {
        Task BulkAsync(List<string> ideasJson);
    }
}