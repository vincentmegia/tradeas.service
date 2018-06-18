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
    public class SecurityRepository : MyCouchClient, ISecurityRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransactionRepository));

        public SecurityRepository(string serverAddress) : base(serverAddress, "securities")
        {
        }


        /// <summary>
        /// Bulks the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="brokerTransactions">Ideas json.</param>
        public async Task<Result> GetAll()
        {
            var queryViewRequest = new QueryViewRequest("query", "all")
                .Configure(c => c.IncludeDocs(true));
            var response = await Views.QueryAsync<Security>(queryViewRequest);

            var securities = response
                .Rows
                .Select(row =>
                    (Security) JsonConvert.DeserializeObject(row.IncludedDoc,
                        typeof(Security))) // iad to resort to this, freakin framework works finicky
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