using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Tradeas.Models;
using Tradeas.Repositories;

namespace Tradeas.Colfinancial.Provider.Processors
{
    public class BrokerTransactionProcessor
    {
        private readonly ITradeasRepository _tradeasRepository;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerTransactionProcessor));

        public BrokerTransactionProcessor(ITradeasRepository tradeasRepository)
        {
            _tradeasRepository = tradeasRepository;
        }


        /// <summary>
        /// Process the specified t.
        /// </summary>
        /// <returns>The process.</returns>
        /// <param name="brokerTransactions">T.</param>
        public TaskResult Process(List<BrokerTransaction> brokerTransactions)
        {
            //collect new created transactions
            var jsonList = new List<string>();
            foreach (var brokerTransaction in brokerTransactions)
            {
                var json = JsonConvert.SerializeObject(new BrokerTransactionJson(brokerTransaction), new IsoDateTimeConverter {DateTimeFormat = Models.Constants.DateFormat});
                Logger.Info($"converted json: {json}");
                jsonList.Add(json);
            }

            _tradeasRepository.BulkAsync(jsonList);
            var taskResult = new TaskResult { IsSuccessful = true };
            taskResult.SetData(brokerTransactions);
            return taskResult;
        }

        /// <summary>
        /// Process the specified t.
        /// </summary>
        /// <returns>The process.</returns>
        /// <param name="t">T.</param>
        public Task<Result> Process(Transaction t)
        {
            throw new System.NotImplementedException();
        }
    }
}
