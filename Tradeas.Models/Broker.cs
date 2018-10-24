using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class Broker
    {
        public virtual string Id { get; set; }
        public virtual string Rev { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("documentType")]
        public string DocumentType => "broker";

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Code)}: {Code}";
        }
    }
}