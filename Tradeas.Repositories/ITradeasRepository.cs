using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface ITradeasRepository : IRepository
    {
        Task<Result> GetUser(string username);
    }
}