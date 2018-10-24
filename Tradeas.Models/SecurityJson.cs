using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class SecurityJson : Security
    {
        [JsonProperty("_id")]
        public override string Id { get; set; }
        
        [JsonProperty("_rev")]
        public override string Rev { get; set; }

        public SecurityJson(Security security)
        {
            Id = security.Id;
            Rev = security.Rev;
            CompanyName = security.CompanyName;
            Symbol = security.Symbol;
            Sector = security.Sector;
            SubSector = security.SubSector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRev()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (Rev != null);
        }
    }
}