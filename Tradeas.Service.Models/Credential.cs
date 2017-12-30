namespace Tradeas.Service.Models
{
    public class Credential
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Credential() {}

        public override string ToString()
        {
            return string.Format("[Credential: Username={0}, Password={1}]", Username, Password);
        }
    }
}