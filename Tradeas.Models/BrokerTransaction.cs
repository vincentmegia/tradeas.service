using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class BrokerTransaction
    {
        public virtual string Id { get; set; }
        public virtual string Rev { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("createdDate")]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime? UpdatedDate { get; set; }
        
        [JsonProperty("updatedBy")]
        public string UpdatedBy { get; set; }
        
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("documentType")]
        public string DocumentType => "broker-transaction";

        [JsonProperty("details")]
        public List<BrokerTransactionDetail> Details { get; }

        
        public BrokerTransaction()
        {
            Details = new List<BrokerTransactionDetail>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Symbol)}: {Symbol}, {nameof(CreatedDate)}: {CreatedDate}, {nameof(UpdatedDate)}: {UpdatedDate}, {nameof(UpdatedBy)}: {UpdatedBy}, {nameof(CreatedBy)}: {CreatedBy}, {nameof(Details)}: {Details.Count}";
        }
    }

}