using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Tradeas.Colfinancial.Provider.Scrapers;
using Tradeas.Models;
using Tradeas.Repositories;

namespace Tradeas.Colfinancial.Provider.Processors
{
    public class BrokerTransactionProcessor
    {
        private readonly IBrokerTransactionRepository _brokerTransactionRepository;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerTransactionProcessor));

        public BrokerTransactionProcessor(IBrokerTransactionRepository brokerTransactionRepository)
        {
            _brokerTransactionRepository = brokerTransactionRepository;
        }


        /// <summary>
        /// Process the specified t.
        /// </summary>
        /// <returns>The process.</returns>
        /// <param name="brokerTransactions">T.</param>
        public async Task<Result> Process(List<BrokerTransaction> brokerTransactions)
        {
            //collect new created transactions
            var jsonList = new List<string>();
            foreach (var brokerTransaction in brokerTransactions)
            {
                var json = JsonConvert.SerializeObject(brokerTransaction, new IsoDateTimeConverter {DateTimeFormat = Models.Constants.DateFormat});
                Logger.Info($"converted json: {json}");
                jsonList.Add(json);
            }

            await _brokerTransactionRepository.BulkAsync(jsonList);
            var taskResult = new TaskResult { IsSuccessful = true }.SetData(brokerTransactions);
            return taskResult;
        }

        /// <summary>
        /// Process the specified t.
        /// </summary>
        /// <returns>The process.</returns>
        /// <param name="t">T.</param>
        public async Task<Result> Process(Transaction t)
        {
            throw new System.NotImplementedException();
        }
    }
}
