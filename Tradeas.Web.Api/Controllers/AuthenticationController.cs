using System.Linq;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
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
            
            //ensure other fields are not exposed
            result.Id = null;
            result.Rev = null;
            result.FirstAccess = null;
            result.Email = null;
            result.FirstName = null;
            result.LastName = null;
            result.Address = null;
            result.City = null;
            result.PostalCode = null;
            result.AboutMe = null;
            result.Password = null;
            result.Company = null;
            HttpContext.Response.Headers.Add("Set-Cookie", result.Cookie);
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("validate")]
        public IActionResult Validate([FromBody]User user)
        {            
            user = _authenticationService.Validate(user);
            if (user != null) return Ok(user);
            
            HttpContext.Response.Headers.Remove("Set-Cookie");
            HttpContext.Response.Headers.Remove("Authorization");
            return BadRequest(new {message = "token is not valid, immediately terminating session."});
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