namespace MySlApi.Core.Tests.ScreenScrapers
{
    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MySlApi.Core.ScreenScrapers;
    using MySlApi.Core.Tests.Helpers;

    [TestClass]
    public class LoginPageScraperTests
    {
        [TestMethod]
        public void ScrapePostParametersShouldReturnAllHiddenInputFields()
        {
            // Arrange
            var html = ResourceFileHelper.GetHtmlFile("Login.html");
            var scraper = new LoginPageScraper(html);

            // Act
            var parameters = scraper.ScrapePostParameters();

            // Assert
            parameters.Count.Should().Be(6);
            parameters.Should().Contain(strings => strings[0] == "__EVENTVALIDATION" && strings[1] == "/wEWCAKV6p3kBgLEvZ3pBwLB5tv4AgLn4tafCgKSqc+oBgKfj9j5AgKri92nDQL7s8yDBtFFlOavFX+iNM9AYzya2UZvZ7oR");
            parameters.Should().Contain(strings => strings[0] == "__VIEWSTATE" && strings[1] == "ViewstateIsForWebForms");
            parameters.Should().Contain(strings => strings[0] == "__EVENTARGUMENT" && strings[1] == string.Empty);
        }
    }
}
