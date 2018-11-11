using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface ITradeasRepository : IRepository
    {
        Task<Result> GetUser(string username);
        Task<Result> Login(string username, string password);
        Task<Result> IsCookieValid(User user);
        Task<Result> DeleteSession(User user);
    }
}