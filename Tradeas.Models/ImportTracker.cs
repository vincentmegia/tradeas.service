using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class ImportTracker : IEqualityComparer<ImportTracker>
    {
        private sealed class SymbolEqualityComparer : IEqualityComparer<ImportTracker>
        {
            public bool Equals(ImportTracker x, ImportTracker y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.Symbol, y.Symbol);
            }

            public int GetHashCode(ImportTracker obj)
            {
                return (obj.Symbol != null ? obj.Symbol.GetHashCode() : 0);
            }
        }

        public static IEqualityComparer<ImportTracker> SymbolComparer { get; } = new SymbolEqualityComparer();
        public bool Equals(ImportTracker x, ImportTracker y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(ImportTracker obj)
        {
            throw new NotImplementedException();
        }

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


        public ImportTracker()
        {}

        public ImportTracker(string symbol) : this(symbol, DateTime.Now)
        {}
        
        public ImportTracker(string symbol, DateTime? dateTime)
        {
            var date = dateTime.HasValue ? dateTime : DateTime.Now;
            Id = $"{symbol}-{date:yyyyMMMdd}";
            Symbol = symbol;
        }
        
        public override bool Equals(object obj)
        {
            var target = (ImportTracker) obj;
            if (target.Symbol.Equals(this.Symbol, StringComparison.CurrentCultureIgnoreCase)) return true;
            return false;
        }

        public override int GetHashCode()
        {
            return this.Symbol.GetHashCode();
        }

        public override string ToString()
        {
            return $"{nameof(Symbol)}: {Symbol}, {nameof(CreatedBy)}: {CreatedBy}, {nameof(CreatedDate)}: {CreatedDate}, {nameof(Status)}: {Status}";
        }
    }
}