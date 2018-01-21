using System.Collections.Generic;
using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface IJournalRepository
    {
        Task<Result<Idea>> GetIdea(Idea idea);
        Task<Result<Idea>> GetIdea(Transaction transaction);
        Task<Result<List<Idea>>> GetIdeasOpenStatus();
        Task BulkAsync(List<string> ideasJson);
    }
}
