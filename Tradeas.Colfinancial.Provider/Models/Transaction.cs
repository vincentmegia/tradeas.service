using System;

namespace Tradeas.Colfinancial.Provider.Models
{
    public class Transaction
    {
        public string _id { get; set; }
        public string Symbol { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? MatchedQuantity { get; set; }
        public decimal? Price { get; set; }
        public string Side { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return string.Format("[Transaction: Symbol={0}, Quantity={1}, MatchedQuantity={2}, Price={3}, Side={4}, Status={5}]", Symbol, Quantity, MatchedQuantity, Price, Side, Status);
        }
    }
}
