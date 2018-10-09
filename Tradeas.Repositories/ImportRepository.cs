using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using MyCouch;
using MyCouch.Net;
using MyCouch.Requests;
using Tradeas.Models;
using Newtonsoft.Json;

namespace Tradeas.Repositories
{
    public class ImportRepository : MyCouchClient, IImportRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransactionRepository));

        public ImportRepository(string serverAddress) : base(serverAddress, "imports")
        {}

        
        /// <summary>
        /// Bulks the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="imports">Ideas json.</param>
        public async Task<Result> BulkAsync(List<string> imports)
        {
            Logger.Info($"performing bulk imports for broker transaction total: {imports.Count}");
            var request = new BulkRequest();
            request.Include(imports.ToArray());
            var response = await Documents.BulkAsync(request);
            Logger.Info($"operaton reason: {response.Reason}");
            return new TaskResult {IsSuccessful = true};
        }
        
        /// <summary>
        /// Bulks the async.
        /// </summary>
        /// <returns>The async.</returns>
        public async Task<Result> GetAll()
        {
            var queryViewRequest = new QueryViewRequest("query", "all")
                .Configure(c => c.IncludeDocs(true));
            var response = await Views.QueryAsync<Import>(queryViewRequest);

            var imports = response
                .Rows
                .Select(row => row.Value) // iad to resort to this, freakin framework works finicky
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
                .SetData(imports);
            return result;
        }
   }
}