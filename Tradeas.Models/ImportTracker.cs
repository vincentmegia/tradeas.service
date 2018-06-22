using System;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class ImportTracker
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }
        
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate => DateTime.Now;

        public override bool Equals(object obj)
        {
            var target = (ImportTracker) obj;
            if (target.Symbol.Equals(this.Symbol, StringComparison.CurrentCultureIgnoreCase)) return true;
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(Symbol)}: {Symbol}, {nameof(CreatedBy)}: {CreatedBy}, {nameof(CreatedDate)}: {CreatedDate}";
        }
    }
}