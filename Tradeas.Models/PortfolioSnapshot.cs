using System;

namespace Tradeas.Models
{
    public class PortfolioSnapshot
    {
        public string BrokerCode { get; set; }
        public decimal? TotalEquity { get; set; }
        public decimal? Balance { get; set; }
        public decimal? BuyingPower { get; set; }
        public decimal? GainLossValue { get; set; }
        public decimal? GainLossPercentage { get; set; }
        public decimal? DayChangeValue { get; set; }
        public decimal? DayChangePercentage { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}