namespace MySlApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class AuthenticateRequestModel
    {
        [DataMember, Required]
        public string Username { get; set; }

        [DataMember, Required]
        public string Password { get; set; }
    }
}