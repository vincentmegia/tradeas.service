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