namespace Tradeas.Models
{
    public class TransactionParameter
    {
        public Credential LoginCredential { get; set; }
        public string Frequency { get; set; }

        public TransactionParameter()
        {}

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.TransactionParameter"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.TransactionParameter"/>.</returns>
        public override string ToString()
        {
            return string.Format("[TransactionParameter: LoginCredential={0}, Frequency={1}]", LoginCredential, Frequency);
        }
    }
}