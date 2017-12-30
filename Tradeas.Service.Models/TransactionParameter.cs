namespace Tradeas.Service.Models
{
    public class TransactionParameter
    {
        public Credential LoginCredential { get; set; }
        public string Frequency { get; set; }

        public TransactionParameter()
        {}
    }
}