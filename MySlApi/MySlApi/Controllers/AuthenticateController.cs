namespace MySlApi.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    using MySlApi.Core;
    using MySlApi.Models;

    public class AuthenticateController : ApiController
    {
        public async Task<AuthenticateResponseModel> Post(AuthenticateRequestModel requestModel)
        {
            var scraper = new SlScreenScraper(requestModel.Username, requestModel.Password);
            
            var success = await scraper.Authenticate();

            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            return new AuthenticateResponseModel { Authenticated = true, CookieHeader = scraper.GetCookieHeader() };
        }
    }
}