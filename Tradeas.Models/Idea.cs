using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class Idea
    {
        /// <summary>
        /// My Couch framework will populate this with _id
        /// </summary>
        /// <value>The document identifier.</value>
        public string DocumentId
        {
            get { return Id; }
            set { Id = value; }
        }


        /// <summary>
        /// MYCOUCH framework will populate this with _rev
        /// </summary>
        /// <value>The document rev.</value>
        public string DocumentRev
        {
            get { return Revision; }
            set { Revision = value; }
        }

        [JsonProperty(PropertyName = "_id")] 
        public string Id { get; set; }

        [JsonProperty(PropertyName = "_rev")] 
        public string Revision { get; set; }

        [JsonProperty(PropertyName = "symbol")] 
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "type")] 
        public string Type { get; set; }

        [JsonProperty(PropertyName = "chart")] 
        public string Chart{ get; set; }

        [JsonProperty(PropertyName = "entryDate")]
        public DateTime EntryDate { get; set; }

        [JsonProperty(PropertyName = "stars")] 
        public List<Star> Stars { get; set; }

        [JsonProperty(PropertyName = "status")] 
        public string Status { get; set; }

        [JsonProperty(PropertyName = "position")]
        public Position Position { get; set; }


        /// <summary>
        /// Shoulds the serialize revision.
        /// </summary>
        /// <returns><c>true</c>, if serialize revision was shoulded, <c>false</c> otherwise.</returns>
        public bool ShouldSerializeRevision()
        {
            return !string.IsNullOrEmpty(Revision);
        }

        public bool ShouldSerializeDocumentId()
        {
            return false;
        }

        public bool ShouldSerializeDocumentRev()
        {
            return false;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.Idea"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.Idea"/>.</returns>
        public override string ToString()
        {
            return string.Format("[Idea: Id={0}, Revision={1}, Symbol={2}, Type={3}, Chart={4}, Status={5}, EntryDate={6}]", Id, Revision, Symbol, Type, Chart, Status, EntryDate);
        }
    }
}
