namespace Tradeas.Service.Invoker
{
    public class TransactionParameter
    {
        public string Frequency { get; set; }
        public Credential LoginCredential { get; set; }

        public class Credential
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}