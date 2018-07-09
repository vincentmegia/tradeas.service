using System;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class ImportHistory
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; }
        
        [JsonProperty(PropertyName = "type")]
        public string Type { get; }
        
        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; }

        public ImportHistory(string type, string createdBy)
        {
            Id = DateTime.Now.ToString("yyyy-MM-dd");
            Type = type;
            CreatedBy = createdBy;
            CreatedDate = DateTime.Now;
        }
        
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Type)}: {Type}, {nameof(CreatedBy)}: {CreatedBy}, {nameof(CreatedDate)}: {CreatedDate}";
        }
    }
}