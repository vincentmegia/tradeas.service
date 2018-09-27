
using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface IUserRepository : IRepository
    {
        Task<Result> GetUser(string username);
    }
}