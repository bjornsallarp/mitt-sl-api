namespace MySlApi.Core.Tests.ScreenScrapers
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MySlApi.Core.ScreenScrapers;
    using MySlApi.Core.Tests.Helpers;

    [TestClass]
    public class CardsPageScraperTests
    {
        [TestMethod]
        public void ScrapeAllCardsTest()
        {
            // Arrange
            string html = ResourceFileHelper.GetHtmlFile("Cards.html");

            var scraper = new CardsPageScraper(html);
            
            // Act
            var accounts = scraper.ScrapeAllCards();

            // Assert
            accounts.Should().NotBeNull();
            accounts.Count.Should().Be(2);

            accounts.Should()
                    .Contain(card => card.Name == "Annas kort" && card.CardNumber == 123456789 && card.Balance == 0);

            var bjornsCard = accounts.First(card => card.CardNumber == 987654321);
            bjornsCard.Name.Should().Be("Björns kort");
            bjornsCard.Balance.Should().Be(0);
            bjornsCard.Tickets.Count.Should().Be(1);

            var ticket = bjornsCard.Tickets.First();
            ticket.Name.Should().Be("30-dagarsbiljett Helt");
            ticket.Price.Should().Be(790);
            ticket.ValidFrom.Should().Be(DateTime.Parse("2013-03-11"));
            ticket.Expires.Should().Be(DateTime.Parse("2013-04-09"));
        }
    }
}
