using System;
using Newtonsoft.Json;

namespace Tradeas.Colfinancial.Provider.Models
{
    public class Transaction
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "transactionId")]
        public long TransactionId { get; set; }

        [JsonProperty(PropertyName = "orderId")]
        public long OrderId { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public decimal? Quantity { get; set; }

        [JsonProperty(PropertyName = "matchedQuantity")]
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
            return string.Format("[Transaction: Id={0}, TransactionId={1}, OrderId={2}, Symbol={3}, Quantity={4}, MatchedQuantity={5}, Price={6}, Side={7}, Status={8}, CreatedDate={9}]", 
                                 Id, TransactionId, OrderId, Symbol, Quantity, MatchedQuantity, Price, Side, Status, CreatedDate);
        }
    }
}
