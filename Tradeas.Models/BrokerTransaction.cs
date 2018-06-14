using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class BrokerTransaction
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime? CreatedDate => DateTime.Now;

        [JsonProperty(PropertyName = "updatedDate")]
        public DateTime? UpdatedDate { get; set; }
        
        [JsonProperty(PropertyName = "updatedBy")]
        public string UpdatedBy { get; set; }
        
        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "details")]
        public List<BrokerTransactionDetail> Details { get; }

        public BrokerTransaction()
        {
            Details = new List<BrokerTransactionDetail>();
        }
    }
}