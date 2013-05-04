namespace MySlApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using MySlApi.Core;
    using MySlApi.Models;

    public class CardsController : ApiController
    {
        public async Task<IEnumerable<CardResponseModel>> Post(CardsRequestModel requestModel)
        {
            var scraper = new SlScreenScraper(requestModel.CookieHeader);

            try
            {
                var cards = await scraper.GetAccessCards(requestModel.PartyRef);

                return cards.Select(card => new CardResponseModel(card));
            }
            catch (HttpResponseException)
            {   
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }
        }
    }
}
