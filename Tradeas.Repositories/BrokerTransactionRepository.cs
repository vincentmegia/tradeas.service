using System.Collections.Generic;
using log4net;
using MyCouch;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public class BrokerTransactionRepository : MyCouchClient, IBrokerTransactionRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerTransactionRepository));
        
        public BrokerTransactionRepository(string serverAddress) : base(serverAddress, "broker-transactions")
        {}

        public TaskResult BulkAsync(List<string> items)
        {
            throw new System.NotImplementedException();
        }
    }
}