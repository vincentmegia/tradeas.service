using System;
using System.IdentityModel.Tokens.Jwt;
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
        
        public AuthenticationService(IConfiguration configuration,
            ITradeasRepository tradeasRepository)
        {
            _configuration = configuration;
            _tradeasRepository = tradeasRepository;
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
                .GetUser(username)
                .Result
                .GetData<User>();

            if (user == null) return null;
            //decrypt password and compare
            if (!user.Password.Equals(password, StringComparison.InvariantCultureIgnoreCase))
                return null;

            user.Guid = Guid.NewGuid().ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtPassPhrase"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new [] 
                {
                    new Claim("username", user.Username),
                    new Claim("company", user.Company),  
                    new Claim("firstAccess", user.FirstAccess.ToString()),
                    new Claim("email", user.Email),
                    new Claim("firstname", user.FirstName),
                    new Claim("lastname", user.LastName),
                    new Claim("address", user.Address),
                    new Claim("city", user.City),
                    new Claim("postal", user.PostalCode),
                    new Claim("aboutMe", user.AboutMe),
                    new Claim("Guid", user.Guid)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            var response = ((Repository) _tradeasRepository).Entities.PostAsync(user);
            
            user.Id = null;
            user.Rev = null;
            user.FirstAccess = null;
            user.Email = null;
            user.FirstName = null;
            user.LastName = null;
            user.Address = null;
            user.City = null;
            user.PostalCode = null;
            user.AboutMe = null;
            user.Password = null;
            user.Company = null;
            
            //write to users database
            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string Validate(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtPassPhrase"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new [] 
                {
                    new Claim("oldToken", token)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var renewedToken = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(renewedToken);
            return tokenString;
        }
    }
}