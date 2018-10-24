using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCouch.Requests;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public class TradeasRepository : Repository, ITradeasRepository
    {
        public TradeasRepository(string serverAddress) : base(serverAddress, "tradeas")
        {}
        
        /// <summary>
        /// Gets the idea open status.
        /// </summary>
        /// <returns>The idea open status.</returns>
        public async Task<Result> GetUser(string username)
        {
            var queryViewRequest = new QueryViewRequest("users", "by-username").Configure(c => 
                c.Key(username).IncludeDocs(true));
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
            if (response.RowCount == 0) return taskResult;
            
            var user = response.Rows.First().Value;
            taskResult.SetData(user);
            return taskResult;
        }
    }
}