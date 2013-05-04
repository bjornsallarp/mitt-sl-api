namespace MySlApi.Core.Tests.JsonDtoTests
{
    using System.Linq;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MySlApi.Core.JsonDto;
    using MySlApi.Core.Tests.Helpers;

    using Newtonsoft.Json;

    [TestClass]
    public class TravelCardListResponseTests
    {
        [TestMethod]
        public void TravelCardListResponseDtoDeserializationTest()
        {
            // Arrange
            var jsonText = ResourceFileHelper.GetJsonFile("TravelCardListResponse.json");

            // Act
            var json = JsonConvert.DeserializeObject<ResultDataResponse<TravelCardListResponse>>(jsonText);

            // Assert
            json.Result.Cards.Count.Should().Be(2);

            var card = json.Result.Cards.FirstOrDefault(item => item.Card.Href.Equals("travel_card/999999"));
            card.Should().NotBeNull();

            if (card != null)
            {
                card.Card.Name.Should().Be("Annas kort");
                card.Card.SerialNumber.Should().Be("999999999");
            }
        }
    }
}
