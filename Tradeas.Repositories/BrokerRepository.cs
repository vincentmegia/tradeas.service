using log4net;

namespace Tradeas.Repositories
{
    public class BrokerRepository : Repository, IBrokerRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BrokerRepository));
        
        public BrokerRepository(string serverAddress) : base(serverAddress, "brokers")
        {}

    }
}