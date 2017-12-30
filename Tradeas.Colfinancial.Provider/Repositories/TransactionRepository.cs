using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyCouch;
using MyCouch.Requests;

namespace Tradeas.Colfinancial.Provider.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private IMyCouchClient _myCouchClient;

        public TransactionRepository(IMyCouchClient myCouchClient)
        {
            _myCouchClient = myCouchClient;
        }

        /// <summary>
        /// Bulks the insert.
        /// </summary>
        /// <param name="transactions">Transactions.</param>
        public async Task BulkAsync(List<string> transactions)
        {
            var request = new BulkRequest();
            request.Include(transactions.ToArray());
            var response = await _myCouchClient.Documents.BulkAsync(request);
            Console.WriteLine(response.Reason);
        }
    }
}
