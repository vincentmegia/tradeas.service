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
        public async Task<Result<List<Transaction>>> Process(List<Transaction> transactions)
        {
            //collect new created transactions
            var newTransactions = new List<Transaction>();
            foreach (var transaction in transactions)
            {
                var response = await _transactionRepository.PutAsync(transaction);
                if (response.IsSuccessful && response.StatusCode.ToLower() == "created")
                    newTransactions.Add(response.Instance);
            }
            return new Result<List<Transaction>> { IsSuccessful = true, Instance = transactions };
        }

        /// <summary>
        /// Process the specified t.
        /// </summary>
        /// <returns>The process.</returns>
        /// <param name="t">T.</param>
        public async Task<Result<Transaction>> Process(Transaction t)
        {
            throw new System.NotImplementedException();
        }
    }
}
