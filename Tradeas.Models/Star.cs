using System;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    public class Star
    {
        [JsonProperty(PropertyName = "_id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "class")]
        public string Class { get; set; }

        [JsonProperty(PropertyName = "_selected")]
        public string Selected { get; set; }

        public Star()
        {
        }

        public override bool Equals(object obj)
        {
            var target = obj as Star;
            if (this.Id != target.Id) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() * 13;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.Star"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.Star"/>.</returns>
        public override string ToString()
        {
            return string.Format("[Star: Id={0}, Class={1}, Selected={2}]", Id, Class, Selected);
        }
    }
}
