using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class Attachments
    {
        [JsonProperty("profile-logo")]
        public Attachment Attachment { get; set; }
    }
}