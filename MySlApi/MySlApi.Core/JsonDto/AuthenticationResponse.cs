namespace MySlApi.Core.JsonDto
{
    using Newtonsoft.Json;

    internal class AuthenticationResponse
    {
        [JsonProperty(PropertyName = "authentication_session")]
        public AuthenticationResponseAuthenticationSession Session { get; set; }

        internal class AuthenticationResponseAuthenticationSession
        {
            [JsonProperty(PropertyName = "party_ref")]
            public AuthenticationResponsePartyref PartyRef { get; set; }
        }

        internal class AuthenticationResponsePartyref
        {
            [JsonProperty(PropertyName = "ref")]
            public string Ref { get; set; }
        }
    }
}