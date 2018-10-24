using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TransactionRepository : Repository, ITransactionRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransactionRepository));
        private const string DateFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

        public TransactionRepository(string serverAddress) : base(serverAddress, "transactions")
        {}


        /// <summary>
        /// Gets the by keys.
        /// </summary>
        /// <returns>The by keys.</returns>
        /// <param name="keys">Keys.</param>
        public async Task<Result> GetByKeys(List<string> keys)
        {
            var transactions = new List<Transaction>();
            foreach (var key in keys)
            {
                var response = await Entities.GetAsync<Transaction>(key);
                transactions.Add(response.Content);
            }

            var taskResult = new TaskResult { IsSuccessful = true };
            taskResult.SetData(keys);
            return taskResult;
        }

        /// <summary>
        /// Puts the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="transaction">Transaction.</param>
        public async Task<Result> PutAsync(Transaction transaction)
        {
            try
            {
                var json = JsonConvert.SerializeObject(transaction, new IsoDateTimeConverter { DateTimeFormat = DateFormat });
                var response = await Documents.PutAsync(transaction.Id, json);
                if (response.IsSuccess)
                    transaction.Rev = response.Rev;
                Logger.Info(transaction);
                return new TaskResult
                {
                    IsSuccessful = response.IsSuccess,
                    StatusCode = response.StatusCode.ToString(),
                    Reason = response.Reason
                }.SetData(transaction);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return new TaskResult { StatusCode = "Processing" };
        }

        /// <summary>
        /// Gets the orphan transactions.
        /// </summary>
        /// <returns>The orphan transactions.</returns>
        public async Task<Result> GetOrphanTransactions()
        {
            var queryViewRequest = new QueryViewRequest("query", "byPositionId").Configure(query => query.IncludeDocs(true));
            var response = await Views.QueryAsync<Transaction>(queryViewRequest)
                                      as ViewQueryResponse<Transaction>;

            var transactions = response
            .Rows
            .Select(row => (Transaction) JsonConvert.DeserializeObject(row.IncludedDoc, typeof(Transaction))) // iad to resort to this, freakin framework works finicky
            .ToList();
            
            var result = new TaskResult { 
                IsSuccessful = true,
                Messages = new List<string>
                {
                    response.Error,
                    response.Reason
                }}
                .SetData(transactions);
            return result;
        }

        /// <summary>
        /// Gets the orphan transactions.
        /// </summary>
        /// <returns>The orphan transactions.</returns>
        public async Task<Result> GetAll()
        {
            var queryViewRequest = new QueryViewRequest("orphan", "all")
                .Configure(c => c.IncludeDocs(true));
            var response = await Views.QueryAsync<Transaction>(queryViewRequest)
                                      as ViewQueryResponse<Transaction>;

            var transactions = response
                .Rows
                .Select(row => (Transaction)JsonConvert.DeserializeObject(row.IncludedDoc, typeof(Transaction))) // iad to resort to this, freakin framework works finicky
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
                .SetData(transactions);
            return result;
        }
    }
}