using System.Collections.Generic;
using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface IJournalRepository
    {
        Task<Result> GetIdea(Idea idea);
        Task<Result> GetIdea(Transaction transaction);
        Task<Result> GetIdeasOpenStatus();
        Task BulkAsync(List<string> ideasJson);
    }
}
