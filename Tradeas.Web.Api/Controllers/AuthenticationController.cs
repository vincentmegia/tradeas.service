using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tradeas.Models;
using Tradeas.Web.Api.Services;

namespace Tradeas.Web.Api.Controllers
{
    [Route("api/authentication")]
    public class AuthenticationController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AuthenticationController));
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]User user)
        {
            Logger.InfoFormat("performing authentication on user: ", user);
            var result = _authenticationService.Login(user.Username, user.Password);
            if (result == null)
                return BadRequest(new {message = "Incorrect Username or Password"});

            var key = $"{result.Username}:{result.Guid}";
            result.Username = key;
            HttpContext.Session.SetString(key, result.Token);
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("validate")]
        public IActionResult Validate([FromBody]User user)
        {
            var token = HttpContext.Session.GetString(user.Username);
            if (token == null || token != user.Token) return BadRequest("Invalid session token was detected");
            
            user.Token = _authenticationService.Validate(user.Token); 

            return Ok(user);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var users = new User();
            return Ok(users);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }
    }
}