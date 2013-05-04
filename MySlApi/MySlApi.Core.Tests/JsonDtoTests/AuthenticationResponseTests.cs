namespace MySlApi.Core.Tests.JsonDtoTests
{
    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MySlApi.Core.JsonDto;
    using MySlApi.Core.Tests.Helpers;

    using Newtonsoft.Json;

    [TestClass]
    public class AuthenticationResponseTests
    {
        [TestMethod]
        public void AuthenticationResponseDtoDeserializationTest()
        {
            // Arrange
            var jsonText = ResourceFileHelper.GetJsonFile("AuthenticationResponse.json");

            // Act
            var json = JsonConvert.DeserializeObject<ResultDataResponse<AuthenticationResponse>>(jsonText);

            // Assert
            json.Result.Session.PartyRef.Ref.Should().Be("private_customer/999999");
        }
    }
}
