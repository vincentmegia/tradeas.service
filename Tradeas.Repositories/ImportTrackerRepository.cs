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
    public class ImportTrackerRepository : MyCouchClient, IImportTrackerRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerTransactionRepository));

        public ImportTrackerRepository(string serverAddress) : base(serverAddress, "imports-tracker")
        {}

        
        /// <summary>
        /// Bulks the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="importTracker">Ideas json.</param>
        public async Task<Result> PostAsync(ImportTracker importTracker)
        {
            Logger.Info($"adding imported broker transaction tracker: {importTracker}");
            var response = await Entities.PostAsync(importTracker);
            Logger.Info($"operaton status code: {response.StatusCode}");
            return new TaskResult {IsSuccessful = response.IsSuccess};
        }
        
        /// <summary>
        /// Bulks the async.
        /// </summary>
        /// <returns>The async.</returns>
        public async Task<Result> GetAll()
        {
            var queryViewRequest = new QueryViewRequest("query", "all")
                .Configure(c => c.IncludeDocs(true));
            var response = await Views.QueryAsync<ImportTracker>(queryViewRequest);

            if (response.RowCount == 0) return new TaskResult().SetData(new List<ImportTracker>());
            var importTrackers = response
                .Rows
                .Select(row =>
                    (ImportTracker) JsonConvert.DeserializeObject(row.IncludedDoc,
                        typeof(ImportTracker))) // iad to resort to this, freakin framework works finicky
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
                .SetData(importTrackers);
            return result;
        }
   }
}