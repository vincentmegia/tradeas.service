using Tradeas.Models;

namespace Tradeas.Web.Api.Services
{
    public interface IAuthenticationService
    {
        User Login(string username, string password);
        User Validate(User user);
    }
}