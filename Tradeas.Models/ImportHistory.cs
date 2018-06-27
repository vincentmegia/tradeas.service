using System;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class ImportHistory
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "type")]
        public string Symbol { get; }
        
        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; }
        
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate => DateTime.Now;
    }
}