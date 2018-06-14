using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using MyCouch;
using MyCouch.Requests;

namespace Tradeas.Repositories
{
    public class BrokerTransactionRepository : MyCouchClient, IBrokerTransactionRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransactionRepository));

        public BrokerTransactionRepository(string serverAddress) : base(serverAddress, "broker-transactions")
        {}

        
        /// <summary>
        /// Bulks the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="brokerTransactions">Ideas json.</param>
        public async Task BulkAsync(List<string> brokerTransactions)
        {
            Logger.Info($"performing bulk transctions for broker transaction total: {brokerTransactions.Count}");
            var request = new BulkRequest();
            request.Include(brokerTransactions.ToArray());
            var response = await Documents.BulkAsync(request);
            Logger.Info($"operaton reason: {response.Reason}");
        }
   }
}