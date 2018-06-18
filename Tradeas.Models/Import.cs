using System;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class Import
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime? CreatedDate => DateTime.Now;

        [JsonProperty(PropertyName = "updatedDate")]
        public DateTime? UpdatedDate { get; set; }
        
        [JsonProperty(PropertyName = "updatedBy")]
        public string UpdatedBy { get; set; }
        
        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// don't serialize if id is null for PostAsync operations
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeId()
        {
            return !string.IsNullOrEmpty(Id);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Symbol)}: {Symbol}, {nameof(Type)}: {Type}, {nameof(CreatedDate)}: {CreatedDate}, {nameof(UpdatedDate)}: {UpdatedDate}, {nameof(UpdatedBy)}: {UpdatedBy}, {nameof(CreatedBy)}: {CreatedBy}";
        }
    }
}