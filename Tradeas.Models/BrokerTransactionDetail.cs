using System;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class BrokerTransactionDetail
    {
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
        
        [JsonProperty(PropertyName = "buyVolume")]
        public decimal? BuyVolume { get; set; }
        
        [JsonProperty(PropertyName = "buyAmount")]
        public decimal? BuyAmount { get; set; }
        
        [JsonProperty(PropertyName = "buyAverage")]
        public decimal? BuyAverage { get; set; }
        
        [JsonProperty(PropertyName = "sellVolume")]
        public decimal? SellVolume{ get; set; }
        
        [JsonProperty(PropertyName = "sellAmount")]
        public decimal? SellAmount{ get; set; }
        
        [JsonProperty(PropertyName = "sellAverage")]
        public decimal? SellAverage{ get; set; }
        
        [JsonProperty(PropertyName = "netAmount")]
        public decimal? NetAmount{ get; set; }
        
        [JsonProperty(PropertyName = "totalValue")]
        public decimal? TotalValue{ get; set; }
        
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime? CreatedDate => DateTime.Now;

        [JsonProperty(PropertyName = "updatedDate")]
        public DateTime? UpdatedDate { get; set; }
        
        [JsonProperty(PropertyName = "updatedBy")]
        public string UpdatedBy { get; set; }
        
        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Code)}: {Code}, {nameof(BuyVolume)}: {BuyVolume}, {nameof(BuyAmount)}: {BuyAmount}, {nameof(BuyAverage)}: {BuyAverage}, {nameof(SellVolume)}: {SellVolume}, {nameof(SellAmount)}: {SellAmount}, {nameof(SellAverage)}: {SellAverage}, {nameof(NetAmount)}: {NetAmount}, {nameof(TotalValue)}: {TotalValue}, {nameof(CreatedDate)}: {CreatedDate}, {nameof(UpdatedDate)}: {UpdatedDate}, {nameof(UpdatedBy)}: {UpdatedBy}, {nameof(CreatedBy)}: {CreatedBy}";
        }
    }
}