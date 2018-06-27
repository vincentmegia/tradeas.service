using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using MyCouch;
using MyCouch.Requests;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public class ImportHistoryRepository : MyCouchClient, IImportHistoryRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerTransactionRepository));

        public ImportHistoryRepository(string serverAddress) : base(serverAddress, "imports-history")
        {}

        
        /// <summary>
        /// Bulks the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="brokerTransactions">Ideas json.</param>
        public async Task<Result> BulkAsync(List<string> brokerTransactions)
        {
            Logger.Info($"performing bulk transactions for broker with total: {brokerTransactions.Count}");
            var request = new BulkRequest();
            request.Include(brokerTransactions.ToArray());
            var response = await Documents.BulkAsync(request);
            Logger.Info($"operaton status code: {response.StatusCode}");
            return new TaskResult {IsSuccessful = true};
        }
   }
}