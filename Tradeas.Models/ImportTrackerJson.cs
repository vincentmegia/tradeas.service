using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class ImportTrackerJson : ImportTracker, IEqualityComparer<ImportTracker>
    {
        public static IEqualityComparer<ImportTracker> SymbolComparer { get; } = new SymbolEqualityComparer();
        
        [JsonProperty("_id")]
        public override string Id { get; set; }
        
        [JsonProperty("_rev")]
        public override string Rev { get; set; }

        [JsonProperty("$doctype")] public string DocumentType => "importTracker";

        public ImportTrackerJson(ImportTracker importTracker)
        {
            Id = importTracker.Id;
            Rev = importTracker.Rev;
            Symbol = importTracker.Symbol;
            Status = importTracker.Status;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var target = (ImportTracker) obj;
            if (target.Symbol.Equals(this.Symbol, StringComparison.CurrentCultureIgnoreCase)) return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Symbol.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Symbol)}: {Symbol}, {nameof(CreatedBy)}: {CreatedBy}, {nameof(CreatedDate)}: {CreatedDate}, {nameof(Status)}: {Status}";
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Equals(ImportTracker x, ImportTracker y)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetHashCode(ImportTracker obj)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// 
        /// </summary>
        private sealed class SymbolEqualityComparer : IEqualityComparer<ImportTracker>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(ImportTracker x, ImportTracker y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.Symbol, y.Symbol);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(ImportTracker obj)
            {
                return (obj.Symbol != null ? obj.Symbol.GetHashCode() : 0);
            }
        }
    }
}