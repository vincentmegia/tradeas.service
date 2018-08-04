using System;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class ImportTracker
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "_rev")]
        public string Rev { get; set; }
        
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; }
        
        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; }
        
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate => DateTime.Now;

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        
        public ImportTracker(string symbol)
        {
            Id = $"{symbol}-{DateTime.Now:yyyyMMMdd}";
            Symbol = symbol;
        }
        
        public override bool Equals(object obj)
        {
            var target = (ImportTracker) obj;
            if (target.Symbol.Equals(this.Symbol, StringComparison.CurrentCultureIgnoreCase)) return true;
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(Symbol)}: {Symbol}, {nameof(CreatedBy)}: {CreatedBy}, {nameof(CreatedDate)}: {CreatedDate}, {nameof(Status)}: {Status}";
        }
    }
}