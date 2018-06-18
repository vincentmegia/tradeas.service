using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class Security
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "companyName")]
        public string CompanyName { get; set; }
        
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty(PropertyName = "sector")]
        public string Sector { get; set; }
        
        [JsonProperty(PropertyName = "subSector")]
        public string SubSector { get; set; }

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