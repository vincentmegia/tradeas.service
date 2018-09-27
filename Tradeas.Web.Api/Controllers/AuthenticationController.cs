using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tradeas.Models;
using Tradeas.Web.Api.Services;

namespace Tradeas.Web.Api.Controllers
{
    [Route("api/authentication")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        
        // GET
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]User userParameter)
        {
            var user = _authenticationService.Login(userParameter.Username, userParameter.Password);
            if (user == null)
                return BadRequest(new {message = "Incorrect Username or Password"});

            return Ok(user);
        }
    }
}