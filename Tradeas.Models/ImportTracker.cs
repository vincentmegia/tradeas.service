﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class ImportTracker : IEqualityComparer<ImportTracker>
    {
        public static IEqualityComparer<ImportTracker> SymbolComparer { get; } = new SymbolEqualityComparer();
        
        public virtual string Id { get; set; }
        public virtual string Rev { get; set; }
        
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("createdBy")]
        public string CreatedBy { get; }
        
        [JsonProperty("createdDate")]
        public DateTime CreatedDate => DateTime.Now;

        [JsonProperty("status")]
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