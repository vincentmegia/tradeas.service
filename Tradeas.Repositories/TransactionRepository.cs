using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using MyCouch;
using MyCouch.Requests;
using MyCouch.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public class TransactionRepository : MyCouchClient, ITransactionRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransactionRepository));
        private const string DateFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

        public TransactionRepository(string serverAddress) : base(serverAddress, "transactions")
        {}

        /// <summary>
        /// Bulks the insert.
        /// </summary>
        /// <param name="transactions">Transactions.</param>
        public async Task BulkAsync(List<string> transactions)
        {
            var request = new BulkRequest();
            request.Include(transactions.ToArray());
            var response = await Documents.BulkAsync(request);
            Logger.Info("bulk transaction opeartion completed.");
        }

        /// <summary>
        /// Gets the by keys.
        /// </summary>
        /// <returns>The by keys.</returns>
        /// <param name="keys">Keys.</param>
        public async Task<Result<List<Transaction>>> GetByKeys(List<string> keys)
        {
            var transactions = new List<Transaction>();
            foreach (var key in keys)
            {
                var response = await Entities.GetAsync<Transaction>(key);
                transactions.Add(response.Content);
            }

            return new Result<List<Transaction>> { IsSuccessful = true, Instance = transactions };
        }

        public async Task<ResponseResult<Transaction>> PutAsync(Transaction transaction)
        {
            var response = new DocumentHeaderResponse();
            try
            {
                var json = JsonConvert.SerializeObject(transaction, new IsoDateTimeConverter { DateTimeFormat = DateFormat });
                response = await Documents.PutAsync(transaction.Id, json);
                if (response.IsSuccess)
                    transaction.Revision = response.Rev;
                Logger.Info(response);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            var result = new ResponseResult<Transaction> { 
                IsSuccessful = response.IsSuccess, 
                Instance = transaction,
                StatusCode = response.StatusCode.ToString(),
                Reason = response.Reason};
            return result;
        }
    }
}