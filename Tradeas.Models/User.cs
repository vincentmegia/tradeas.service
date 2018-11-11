using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Tradeas.Models
{
    [JsonObject("user")]
    public class User
    {
        public string Id { get; set; }
        public string Rev { get; set; }
        public bool? FirstAccess { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string AboutMe { get; set; }
        public string Token { get; set; }
        public string Guid { get; set; }
        public string Cookie { get; set; }
        public string DocumentType => "user";
        
//        [JsonProperty("_attachments")]
//        public Attachments Attachments { get; set; }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
//        public bool ShouldSerializeAttachments()
//        {
//            // don't serialize the Manager property if an employee is their own manager
//            return (Attachments != null);
//        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeId()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (Id != null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeAboutMe()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (AboutMe != null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializePostalCode()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (PostalCode != null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeCity()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (City != null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeAddress()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (Address != null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeLastName()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (LastName != null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeFirstName()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (FirstName != null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeEmail()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (Email != null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeFirstAccess()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (FirstAccess != null);
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializePassword()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (Password != null);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeCompany()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (Company != null);
        }
        
        public override string ToString()
        {
            return $"{nameof(Username)}: {Username}, {nameof(Password)}: {Password}, {nameof(Token)}: {Token}";
        }
    }
}