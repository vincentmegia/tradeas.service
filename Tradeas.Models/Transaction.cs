using System;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class Transaction
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "_rev")]
        public string Revision { get; set; }

        [JsonProperty(PropertyName = "transactionId")]
        public long? TransactionId { get; set; }

        [JsonProperty(PropertyName = "orderId")]
        public long? OrderId { get; set; }

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

        [JsonProperty(PropertyName = "positionId")]
        public string PositionId { get; set; }

        public Transaction()
        {}

        /// <summary>
        /// Shoulds the serialize revision.
        /// </summary>
        /// <returns><c>true</c>, if serialize revision was shoulded, <c>false</c> otherwise.</returns>
        public bool ShouldSerializeRevision()
        {
            return !string.IsNullOrEmpty(Revision);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:Tradeas.Models.Transaction"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:Tradeas.Models.Transaction"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="T:Tradeas.Models.Transaction"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var target = obj as Transaction;
            if (TransactionId == target.TransactionId) return true;
            return base.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="T:Tradeas.Models.Transaction"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return TransactionId.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.Transaction"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.Transaction"/>.</returns>
        public override string ToString()
        {
            return string.Format("[Transaction: Id={0}, TransactionId={1}, OrderId={2}, Symbol={3}, Quantity={4}, MatchedQuantity={5}, Price={6}, Side={7}, Status={8}, CreatedDate={9}]", 
                                 Id, TransactionId, OrderId, Symbol, Quantity, MatchedQuantity, Price, Side, Status, CreatedDate);
        }
    }
}
