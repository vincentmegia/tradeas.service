using Newtonsoft.Json;

namespace Tradeas.Models
{
    [JsonObject("profile-logo")]
    public class Attachment
    {
        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("revpos")]
        public long Revpos { get; set; }

        [JsonProperty("digest")]
        public string Digest { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("stub")]
        public bool Stub { get; set; }
    }
}