namespace MySlApi.Core.Tests.JsonDtoTests
{
    using System;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MySlApi.Core.JsonDto;
    using MySlApi.Core.Tests.Helpers;

    using Newtonsoft.Json;

    [TestClass]
    public class TravelCardResponseTests
    {
        [TestMethod]
        public void TravelCardResponseDtoDeserializationTest()
        {
            // Arrange
            var jsonText = ResourceFileHelper.GetJsonFile("TravelCardResponse.json");

            // Act
            var json = JsonConvert.DeserializeObject<ResultDataResponse<TravelCardResponse>>(jsonText);

            // Assert
            json.Result.TravelCard.SerialNumber.Should().Be("999999999");
            json.Result.TravelCard.Blocked.Should().BeFalse();
            json.Result.TravelCard.BlockedDate.HasValue.Should().BeFalse();
            json.Result.TravelCard.Name.Should().Be("Björns kort");
            json.Result.TravelCard.Details.ExpireDate.Should().Be(DateTime.Parse("2019-03-06T00:00:00+00:00"));
            json.Result.TravelCard.Details.PurseBlocked.Should().BeFalse();
            json.Result.TravelCard.Details.PurseValue.Should().Be(75);
            json.Result.TravelCard.Details.Status.Should().Be("ISSUED");
        }
    }
}
