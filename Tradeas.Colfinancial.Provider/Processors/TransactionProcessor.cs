using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Tradeas.Models;
using Tradeas.Repositories;

namespace Tradeas.Colfinancial.Provider.Processors
{
    public class TransactionProcessor : ITransactionProcessor
    {
        private readonly ITransactionRepository _transactionRepository;
        private const string DateFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

        public TransactionProcessor(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }


        /// <summary>
        /// Process the specified t.
        /// </summary>
        /// <returns>The process.</returns>
        /// <param name="t">T.</param>
        public async Task<Result> Process(List<Transaction> transactions)
        {
            //collect new created transactions
            var newTransactions = new List<Transaction>();
            foreach (var transaction in transactions)
            {
                if (transaction.PositionId == null)
                    transaction.PositionId = "0";
                var result = await _transactionRepository.PutAsync(transaction) as TaskResult;
                if (result.IsSuccessful.Value && result.StatusCode.ToLower() == "created")
                    newTransactions.Add(result.GetData<Transaction>());
            }
            var taskResult = new TaskResult { IsSuccessful = true }.SetData(transactions);
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
