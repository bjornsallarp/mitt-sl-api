namespace MySlApi.Core.JsonDto
{
    using Newtonsoft.Json;

    internal class AuthenticationRequest
    {
        public AuthenticationRequest()
        {
            this.PostData = new AutenticationRequestAuthData();

            this.Redirect = new AutenticationRequestRedirect { RedirectUrl = "/sv/Resenar/Mitt-SL/MittSL-Oversikt/" };

            this.FormName = "Authenticate";
        }

        public AuthenticationRequest(string username, string password)
            : this()
        {
            this.PostData.Password = password;

            this.PostData.Username = username;
        }

        [JsonProperty(PropertyName = "redirect")]
        public AutenticationRequestRedirect Redirect { get; set; }

        [JsonProperty(PropertyName = "post_data")]
        public AutenticationRequestAuthData PostData { get; set; }

        [JsonProperty(PropertyName = "form_name")]
        public string FormName { get; set; }

        internal class AutenticationRequestRedirect
        {
            [JsonProperty(PropertyName = "200")]
            public string RedirectUrl { get; set; }
        }

        internal class AutenticationRequestAuthData
        {
            [JsonProperty(PropertyName = "username")]
            public string Username { get; set; }

            [JsonProperty(PropertyName = "password")]
            public string Password { get; set; }
        }
    }
}
