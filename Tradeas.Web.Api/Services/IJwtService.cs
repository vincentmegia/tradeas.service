using Tradeas.Models;

namespace Tradeas.Web.Api.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        string RegenerateToken(string token);
    }
}