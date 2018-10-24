using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class BrokerJson : Broker
    {
        [JsonProperty("_id")]
        public override string Id { get; set; }
        
        [JsonProperty("_rev")]
        public override string Rev { get; set; }
        
        public BrokerJson(Broker broker)
        {
            Id = broker.Id;
            Rev = broker.Rev;
            Code = broker.Code;
            Name = broker.Name;
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