using log4net;

namespace Tradeas.Repositories
{
    public class BrokerTransactionRepository : Repository, IBrokerTransactionRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerTransactionRepository));
        
        public BrokerTransactionRepository(string serverAddress) : base(serverAddress, "broker-transactions")
        {}

    }
}