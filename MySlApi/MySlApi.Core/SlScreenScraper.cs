namespace MySlApi.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Authentication;
    using System.Threading.Tasks;
    using System.Web;

    using MySlApi.Core.Entities;
    using MySlApi.Core.ScreenScrapers;

    public class SlScreenScraper
    {
        private const string BaseAddress = "https://sl.se/";

        private const string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.8; rv:18.0) Gecko/20100101 Firefox/18.0";

        private SlScreenScraper()
        {
            this.Cookies = new CookieContainer();

            var handler = new HttpClientHandler
                              {
                                  CookieContainer = this.Cookies,
                                  AllowAutoRedirect = false
                              };

            this.HttpClient = new HttpClient(handler);
            this.HttpClient.BaseAddress = new Uri(BaseAddress);
            this.HttpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            this.HttpClient.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, */*");
        }

        public SlScreenScraper(string username, string password) 
            : this()
        {
            this.Username = username;

            this.Password = password;
        }

        public SlScreenScraper(string cookies) 
            : this()
        {
            this.Cookies.SetCookies(this.HttpClient.BaseAddress, cookies);
        }

        public SlScreenScraper(CookieContainer cookies)
            : this()
        {
            if (cookies != null)
            {
                this.Cookies.Add(cookies.GetCookies(this.HttpClient.BaseAddress));
            }
        }

        private string Username { get; set; }

        private string Password { get; set; }

        private CookieContainer Cookies { get; set; }

        public HttpClient HttpClient { get; set; }

        public async Task<bool> Authenticate()
        {
            const string LoginPath = "/sv/Resenar/Mitt-SL/Mitt-SL/";

            var loginPageResponse = await this.HttpClient.GetAsync(LoginPath).ConfigureAwait(false);

            if (loginPageResponse.IsSuccessStatusCode)
            {
                var loginPageHtml = await loginPageResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var parser = new LoginPageScraper(loginPageHtml);

                var parameters = parser.ScrapePostParameters();
                parameters.Add(new[] { "ctl00$MainContentRegion$MainBodyRegion$UserLogIn$UsernameBox", this.Username });
                parameters.Add(new[] { "ctl00$MainContentRegion$MainBodyRegion$UserLogIn$PasswordBox", this.Password });
                parameters.Add(new[] { "ctl00$MainContentRegion$MainBodyRegion$UserLogIn$btnLogin", "Logga in" });
                parameters.Add(new[] { "ctl00$HelperMenu$iptSearch", string.Empty });

                var post = await this.HttpClient.PostAsync(LoginPath, this.PostParametersToPostData(parameters)).ConfigureAwait(false);

                if (post.StatusCode == HttpStatusCode.Found && post.Headers.Location != null
                    && post.Headers.Location.AbsolutePath == "/sv/Resenar/Mitt-SL/MittSL-Oversikt/")
                {
                    return true;
                }
            }
        
            return false;
        }

        public async Task<List<AccessCard>> GetAccessCards()
        {
            if (this.Cookies.Count == 0)
            {
                throw new AuthenticationException("You need to authenticate before getting access cards!");
            }

            const string CardsPath = "/sv/Resenar/Mitt-SL/SL-Accesskort1/";

            var cardsPageResponse = await this.HttpClient.GetAsync(CardsPath).ConfigureAwait(false);

            if (cardsPageResponse.IsSuccessStatusCode)
            {
                var cardsPageHtml = await cardsPageResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                var scraper = new CardsPageScraper(cardsPageHtml);
                return scraper.ScrapeAllCards();
            }
            
            throw new HttpRequestException("Request faild, response code: " + cardsPageResponse.StatusCode);
        }

        public string GetCookieHeader()
        {
            return this.Cookies.GetCookieHeader(this.HttpClient.BaseAddress);
        }

        private StringContent PostParametersToPostData(IEnumerable<string[]> postParameters)
        {
            var postString = string.Join(
                "&", postParameters.Select(strings => strings[0] + "=" + HttpUtility.UrlEncode(strings[1])));

            var content = new StringContent(postString, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            return content;
        }
    }
}
