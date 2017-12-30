using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tradeas.Colfinancial.Provider.Models;
using Tradeas.Colfinancial.Provider.Repositories;

namespace Tradeas.Colfinancial.Provider.Processors
{
    public class DatabaseProcessor : IDatabaseProcessor
    {
        private readonly ITransactionRepository _transactionRepository;

        public DatabaseProcessor(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        /// <summary>
        /// Bulks the insert.
        /// </summary>
        /// <param name="transactions">Transactions.</param>
        public async Task BulkAsync(List<Transaction> transactions)
        {
            try
            {
                var jsonList = new List<string>();
                foreach (var transaction in transactions)
                {
                    var json = JsonConvert.SerializeObject(transaction);
                    jsonList.Add(json);
                }
                await _transactionRepository.BulkAsync(jsonList);
            }
            catch(Exception e)
            {}
            finally
            {}
        }
    }
}
