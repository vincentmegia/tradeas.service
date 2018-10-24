using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class BrokerTransactionJson : BrokerTransaction
    {
        [JsonProperty("_id")]
        public override string Id { get; set; }

        [JsonProperty("_rev")]
        public override string Rev { get; set; }

        public BrokerTransactionJson(BrokerTransaction brokerTransaction)
        {
            Id = brokerTransaction.Id;
            Rev = brokerTransaction.Rev;
            Symbol = brokerTransaction.Symbol;
            CreatedDate = brokerTransaction.CreatedDate;
            UpdatedDate = brokerTransaction.UpdatedDate;
            UpdatedBy = brokerTransaction.UpdatedBy;
            CreatedBy = brokerTransaction.CreatedBy;
            Details.Clear();
            Details.AddRange(brokerTransaction.Details);
        }
        
        public bool ShouldSerializeRev()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (Rev != null);
        }
    }
}