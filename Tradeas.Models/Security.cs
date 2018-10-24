using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class Security
    {
        public virtual string Id { get; set; }
        public virtual string Rev { get; set; }
        
        [JsonProperty("companyName")]
        public string CompanyName { get; set; }
        
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("sector")]
        public string Sector { get; set; }
        
        [JsonProperty("subSector")]
        public string SubSector { get; set; }

        [JsonProperty("documentType")]
        public string DocumentType => "security";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(CompanyName)}: {CompanyName}, {nameof(Symbol)}: {Symbol}, {nameof(Sector)}: {Sector}, {nameof(SubSector)}: {SubSector}";
        }
    }
}