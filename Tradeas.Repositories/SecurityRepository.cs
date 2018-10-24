using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using MyCouch;
using MyCouch.Requests;
using Newtonsoft.Json;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public class SecurityRepository : Repository, ISecurityRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SecurityRepository));

        public SecurityRepository(string serverAddress) : base(serverAddress, "securities")
        {}


        /// <summary>
        /// Bulks the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="brokerTransactions">Ideas json.</param>
        public async Task<Result> GetAll()
        {
            var queryViewRequest = new QueryViewRequest("securities", "all")
                .Configure(c => c.IncludeDocs(true));
            var response = await Views.QueryAsync<Models.Security>(queryViewRequest);

            var securities = response
                .Rows
                .Select(row =>
                    (Models.Security) JsonConvert.DeserializeObject(row.IncludedDoc,
                        typeof(Models.Security))) // iad to resort to this, freakin framework works finicky
                .ToList();

            var result = new TaskResult
                {
                    IsSuccessful = true,
                    Messages = new List<string>
                    {
                        response.Error,
                        response.Reason
                    }
                }
                .SetData(securities);
            return result;
        }
    }
}