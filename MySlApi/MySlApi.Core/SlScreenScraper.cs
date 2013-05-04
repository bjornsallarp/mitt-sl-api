namespace MySlApi.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using MySlApi.Core.Entities;
    using MySlApi.Core.Extensions;
    using MySlApi.Core.JsonDto;

    using Newtonsoft.Json;

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
                                  AllowAutoRedirect = false,
                                  AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                              };

            this.HttpClient = new HttpClient(handler) { BaseAddress = new Uri(BaseAddress) };
            this.HttpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            this.HttpClient.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, */*");
            this.HttpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");
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
            foreach (string cookie in cookies.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                this.Cookies.SetCookies(this.HttpClient.BaseAddress, cookie);         
            }
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

        public async Task<AuthenticationResult> Authenticate()
        {
            const string LoginPath = "/sv/Resenar/Mitt-SL/Mitt-SL/";

            var loginPageResponse = await this.HttpClient.GetAsync(LoginPath).ConfigureAwait(false);

            if (!loginPageResponse.IsSuccessStatusCode)
            {
                return new AuthenticationResult();
            }

            var postData = new AuthenticationRequest(this.Username, this.Password);
            var req = new HttpRequestMessage(HttpMethod.Post, "/ext/mittsl/api/authenticate.json");
            req.Content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

            var post = await this.HttpClient.SendAsync(req).ConfigureAwait(false);

            if (post.StatusCode == HttpStatusCode.OK)
            {
                var response = await post.Content.ReadJsonAsTypeAsync<ResultDataResponse<AuthenticationResponse>>();

                if (!string.IsNullOrEmpty(response.Result.Session.PartyRef.Ref))
                {
                    return new AuthenticationResult
                               {
                                   Authenticated = true,
                                   PartyRef = response.Result.Session.PartyRef.Ref
                               };
                }
            }
        
            return new AuthenticationResult();
        }

        public async Task<List<AccessCard>> GetAccessCards(string partyRef)
        {
            string cardsPath = "/ext/mittsl/api/travel_card.json?queryproperty=owner.ref&value=" + partyRef;

            var cardsPageResponse = await this.HttpClient.GetAsync(cardsPath).ConfigureAwait(false);

            if (!cardsPageResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Request faild, response code: " + cardsPageResponse.StatusCode);                
            }

            var cards = await cardsPageResponse.Content.ReadJsonAsTypeAsync<ResultDataResponse<TravelCardListResponse>>();

            var accessCards = new List<AccessCard>();

            foreach (var card in cards.Result.Cards)
            {
                var cardDetailsUrl = "/ext/mittsl/api/" + card.Card.Href + ".json";
                var detailsResponse = await this.HttpClient.GetAsync(cardDetailsUrl);
                var details = await detailsResponse.Content.ReadJsonAsTypeAsync<ResultDataResponse<TravelCardResponse>>();

                var travelCard = details.Result.TravelCard;

                var accessCard = new AccessCard
                                     {
                                         Blocked = travelCard.Blocked,
                                         BlockedAt = travelCard.BlockedDate,
                                         CardNumber = travelCard.SerialNumber,
                                         Name = travelCard.Name,
                                         CardStatus = travelCard.Details.Status,
                                         ExpireDate = travelCard.Details.ExpireDate,
                                         PurseBalance = travelCard.Details.PurseValue,
                                         PurseBlocked = travelCard.Details.PurseBlocked
                                     };

                if (travelCard.Products != null)
                {
                    accessCard.Tickets =
                        travelCard.Products.Select(product => new Ticket
                                                                  {
                                                                      Name = product.ProductType,
                                                                      Active = product.Active,
                                                                      Blocked = product.Blocked,
                                                                      Expires = product.EndDate,
                                                                      Price = product.ProductPrice,
                                                                      ValidFrom = product.StartDate
                                                                  }).ToList();
                }

                accessCards.Add(accessCard);
            }

            return accessCards;
        }

        public string GetCookieHeader()
        {
            return this.Cookies.GetCookieHeader(this.HttpClient.BaseAddress);
        }
    }
}
