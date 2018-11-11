using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Tradeas.Models;
using Tradeas.Repositories;

namespace Tradeas.Web.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AuthenticationService));
        private readonly IConfiguration _configuration;
        private readonly ITradeasRepository _tradeasRepository;
        private readonly IJwtService _jwtService;
        
        public AuthenticationService(IConfiguration configuration,
            ITradeasRepository tradeasRepository,
            IJwtService jwtService)
        {
            _configuration = configuration;
            _tradeasRepository = tradeasRepository;
            _jwtService = jwtService;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Login(string username, string password)
        {
            //put couchdb code to retrieve here
            //authentication successful so generate jwt token
            var user = _tradeasRepository
                .Login(username, password)
                .Result
                .GetData<User>();

            if (user == null) return null;
            user.Username = username;
            user.Guid = Guid.NewGuid().ToString();
            user.Token = _jwtService.GenerateToken(user);
            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User Validate(User user)
        {
            //verify token is same of username by decoding it.
            var response = _tradeasRepository.IsCookieValid(user);
            if (!response.Result.IsSuccessful.Value)
            {
                _tradeasRepository.DeleteSession(user);
                //delete session
                user.Cookie = null;
                user.Token = null;
                return null;
            }
            user.Token = _jwtService.RegenerateToken(user.Token);
            return user;
        }
    }
}