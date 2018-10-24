namespace Tradeas.Repositories
{
    public class BrokerRepository : Repository, IBrokerRepository
    {
        public BrokerRepository(string serverAddress) : base(serverAddress, "tradeas")
        {}
    }
}