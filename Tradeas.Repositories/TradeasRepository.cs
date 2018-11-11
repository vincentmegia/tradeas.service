using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using log4net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public class TradeasRepository : Repository, ITradeasRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TradeasRepository));
        private readonly RestClient _restClient;

        public TradeasRepository(string serverAddress) : base(serverAddress, "tradeas")
        {
            _restClient = new RestClient(serverAddress) { CookieContainer = new CookieContainer() };
        }
        
        /// <summary>
        /// Gets the idea open status.
        /// </summary>
        /// <returns>The idea open status.</returns>
        public async Task<Result> GetUser(string username)
        {
            var request = new RestRequest(string.Format("_users/org.couchdb.user:{0}", username));
            request.AddHeader("Content-Type", "application/json");
            request.Method = Method.GET;
            var response = _restClient.Execute(request);
            var userMeta = (UserMeta)JsonConvert.DeserializeObject(response.Content, typeof(UserMeta));
            
            var taskResult = new TaskResult
            {
                IsSuccessful = response.IsSuccessful,
                Messages = new List<string>
                {
                    response.ErrorMessage,
                }
            };
            taskResult.SetData(userMeta.User);
            return taskResult;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Result> Login(string username, string password)
        {
            var request = new RestRequest("_session") {Method = Method.POST, RequestFormat = DataFormat.Json};
            request
                .AddHeader("Content-Type", "application/json")
                .AddJsonBody(new {name = username, password = password});

            _restClient.Authenticator = new HttpBasicAuthenticator(username, password);
            var response = _restClient.Execute(request);
            
            Logger.Info($"response: {response.Content}");
            var isUserValid = response.Content.Contains("\"ok\":true");
            var user = (isUserValid)
                ? GetUser(username)
                    .Result
                    .GetData<User>()
                : null;
            
            var taskResult = new TaskResult
            {
                IsSuccessful = isUserValid,
                Messages = new List<string>
                {
                    response.ErrorMessage,
                }
            };
            user.Cookie = response
                .Headers
                .First(header => header.Name == "Set-Cookie")
                .Value
                .ToString();
            taskResult.SetData(user);
            return taskResult;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Result> IsCookieValid(User user)
        {
            var request = new RestRequest("_session") {Method = Method.GET, RequestFormat = DataFormat.Json};
            request
                .AddHeader("Accept", "application/json")
                .AddHeader("Cookie", user.Cookie);

            var response = _restClient.Execute(request);
            
            Logger.Info($"response: {response.Content}");
            var isUserValid = response.Content.Contains("\"ok\":true");
            
            var taskResult = new TaskResult
            {
                IsSuccessful = isUserValid,
                Messages = new List<string>
                {
                    response.ErrorMessage,
                }
            };
            taskResult.SetData(user);
            return taskResult;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Result> DeleteSession(User user)
        {
            var request = new RestRequest("_session") {Method = Method.DELETE, RequestFormat = DataFormat.Json};
            request
                .AddHeader("Accept", "application/json")
                .AddHeader("Cookie", user.Cookie);

            var response = _restClient.Execute(request);
            
            Logger.Info($"response: {response.Content}");
            var isUserValid = response.Content.Contains("\"ok\":true");
            var taskResult = new TaskResult
            {
                IsSuccessful = isUserValid,
                Messages = new List<string>
                {
                    response.ErrorMessage,
                }
            };
            taskResult.SetData(user);
            return taskResult;
        }
    }
}
