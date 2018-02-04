using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class Position
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "transctionId")] 
        public string TransactionId { get; set; }

        [JsonProperty(PropertyName = "orderId")] 
        public string OrderId { get; set; }

        [JsonProperty(PropertyName = "symbol")] 
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "status")] 
        public string Status { get; set; }

        [JsonProperty(PropertyName = "createdDate")] 
        public DateTime? CreatedDate { get; set; }

        [JsonProperty(PropertyName = "transactionIds")] 
        public List<string> TransactionIds { get; set; }

        [JsonProperty(PropertyName = "positionsId")]
        public string PositionId { get; set; }

        [JsonIgnore]
        public decimal? TotalShares { get; set; }

        [JsonIgnore]
        public decimal? AverageBuyPrice { get; set; }

        [JsonIgnore]
        public decimal? AverageSellPrice { get; set; }

        [JsonIgnore]
        public List<Transaction> Transactions { get; set; }


        public Position()
        {
            Transactions = new List<Transaction>();
            TransactionIds = new List<string>();
        }

        /// <summary>
        /// Ises the open.
        /// </summary>
        /// <returns><c>true</c>, if open was ised, <c>false</c> otherwise.</returns>
        public bool IsOpen()
        {
            if (Transactions.Count() == 0) return true;

            var totalBuyQuantity = Transactions
                .Where(transaction => transaction.Side.Equals("buy", StringComparison.CurrentCultureIgnoreCase))
                .Sum(transaction => transaction.MatchedQuantity);
            var totalSellQuantity= Transactions
                .Where(transaction => transaction.Side.Equals("sell", StringComparison.CurrentCultureIgnoreCase))
                .Sum(transaction => transaction.MatchedQuantity);
            var isOpen = Math.Abs(totalBuyQuantity.Value - totalSellQuantity.Value) > 0;
            return isOpen;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.Position"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.Position"/>.</returns>
        public override string ToString()
        {
            return string.Format("[Position: Id={0}, TransactionId={1}, OrderId={2}, Symbol={3}, Status={4}, CreatedDate={5}, TransactionIds={6}, TotalShares={7}, AverageBuyPrice={8}, AverageSellPrice={9}, Transactions={10}]", Id, TransactionId, OrderId, Symbol, Status, CreatedDate, TransactionIds, TotalShares, AverageBuyPrice, AverageSellPrice, Transactions);
        }
    }
}