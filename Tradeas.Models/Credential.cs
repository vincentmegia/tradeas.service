namespace Tradeas.Models
{
    public class Credential
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Credential() {}

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.Credential"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.Credential"/>.</returns>
        public override string ToString()
        {
            return string.Format("[Credential: Username={0}, Password={1}]", Username, Password);
        }
    }
}