using System;
using Newtonsoft.Json;

namespace Tradeas.Colfinancial.Provider.Models
{
    public class Transaction
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public decimal? Quantity { get; set; }

        [JsonProperty(PropertyName = "matchQuantity")]
        public decimal? MatchedQuantity { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal? Price { get; set; }

        [JsonProperty(PropertyName = "side")]
        public string Side { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }

        public override string ToString()
        {
            return string.Format("[Transaction: Id={0}, Symbol={1}, Quantity={2}, MatchedQuantity={3}, Price={4}, Side={5}, Status={6}, CreatedDate={7}]", Id, Symbol, Quantity, MatchedQuantity, Price, Side, Status, CreatedDate);
        }
    }
}
