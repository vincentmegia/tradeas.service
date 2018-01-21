using Tradeas.Strategies;

namespace Tradeas.Colfinancial.Provider
{
    public class TransactionIdStrategy : IdStrategy
    {
        private readonly string _id;

        public TransactionIdStrategy(string id,
                                 string orderId,
                                 string symbol,
                                 string matchedQuantity,
                                 string price,
                                 string broker)
        {
            _id = $"{broker}{id}{orderId}{symbol}{matchedQuantity.Replace(",", string.Empty).Replace(".", string.Empty)}{price.Replace(",", string.Empty).Replace(".", string.Empty)}";
        }

        /// <summary>
        /// Get this instance.
        /// </summary>
        public override string Get()
        {
            return _id;
        }
    }
}
