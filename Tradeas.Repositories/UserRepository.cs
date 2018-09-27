using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCouch;
using MyCouch.Requests;
using Newtonsoft.Json;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public class UserRepository : MyCouchClient, IUserRepository
    {
        public UserRepository(string serverAddress) : base(serverAddress, "users")
        {}

        /// <summary>
        /// Gets the idea open status.
        /// </summary>
        /// <returns>The idea open status.</returns>
        public async Task<Result> GetUser(string username)
        {
            var queryViewRequest = new QueryViewRequest("users", "by-username").Configure(c => c.Key(username).IncludeDocs(true));
            var response = await Views.QueryAsync<User>(queryViewRequest);
            var taskResult = new TaskResult
            {
                IsSuccessful = response.IsSuccess,
                Messages = new List<string>
                {
                    response.Error,
                    response.Reason
                }
            };
            var user = response.Rows.First().Value;
            taskResult.SetData(user);
            return taskResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TaskResult BulkAsync(List<string> items)
        {
            throw new System.NotImplementedException();
        }
    }
}