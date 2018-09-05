using System;

namespace Tradeas.Models
{
    public class TransactionParameter
    {
        public Credential LoginCredential { get; set; }
        public string Frequency { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Symbol { get; set; }
        public bool? NoPurge { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(LoginCredential)}: {LoginCredential}, {nameof(Frequency)}: {Frequency}, {nameof(FromDate)}: {FromDate}, {nameof(ToDate)}: {ToDate}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? GetFromDate()
        {
            return FromDate ?? DateTime.Now;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? GetToDate()
        {
            return ToDate ?? DateTime.Now;
        }
    }
}