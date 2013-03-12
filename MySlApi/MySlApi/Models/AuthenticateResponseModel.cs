namespace MySlApi.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class AuthenticateResponseModel
    {
        [DataMember]
        public bool Authenticated { get; set; }

        [DataMember]
        public string CookieHeader { get; set; }
    }
}