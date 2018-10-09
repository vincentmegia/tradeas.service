using System.Collections.Generic;
using log4net;
using MyCouch;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public class BrokerRepository : MyCouchClient, IBrokerRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerRepository));
        
        public BrokerRepository(string serverAddress) : base(serverAddress, "brokers")
        {}

        public TaskResult BulkAsync(List<string> items)
        {
            throw new System.NotImplementedException();
        }
    }
}