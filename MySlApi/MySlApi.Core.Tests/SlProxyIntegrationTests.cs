namespace MySlApi.Core.Tests
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MySlApi.Core.Entities;

    [TestClass]
    public class SlProxyIntegrationTests
    {
        [TestMethod]
        public async Task AuthenticationShouldReturnCookiesAndPartyRefWithCorrectCredentials()
        {
            if ("Test" == ConfigurationManager.AppSettings["Environment"])
            {
                // Ignore integration tests on appharbour
                return;
            }

            // Arrange
            var proxy = new SlScreenScraper(IntegrationTestsCredentials.Username, IntegrationTestsCredentials.Password);

            // Act
            var result = await proxy.Authenticate();

            // Assert
            result.Authenticated.Should().BeTrue();
            result.PartyRef.Should().NotBeNull();
            proxy.GetCookieHeader().Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public async Task AuthenticationShouldFailWithInvalidCredentials()
        {
            if ("Test" == ConfigurationManager.AppSettings["Environment"])
            {
                // Ignore integration tests on appharbour
                return;
            }

            // Arrange
            var proxy = new SlScreenScraper("fakeusername", "fakepassword");

            // Act
            var result = await proxy.Authenticate();

            // Assert
            result.Authenticated.Should().BeFalse();
            result.PartyRef.Should().BeNullOrEmpty();
        }

        [TestMethod]
        public async Task GetAccessCardsShouldReturnCards()
        {
            if ("Test" == ConfigurationManager.AppSettings["Environment"])
            {
                // Ignore integration tests on appharbour
                return;
            }

            // Arrange
            var proxy = new SlScreenScraper(IntegrationTestsCredentials.Username, IntegrationTestsCredentials.Password);

            // Act
            var authenticationResult = await proxy.Authenticate();
            
            List<AccessCard> cards = null;

            if (authenticationResult.Authenticated)
            {
                cards = await proxy.GetAccessCards(authenticationResult.PartyRef);
            }

            // Assert
            cards.Should().NotBeNull();
            
            if (cards != null)
            {
                cards.Count.Should().BeGreaterThan(0);
            }
        }

        [TestMethod]
        public async Task AuthenticationUsingCookiesShouldWork()
        {
            if ("Test" == ConfigurationManager.AppSettings["Environment"])
            {
                // Ignore integration tests on appharbour
                return;
            }

            // Arrange
            var authScraper = new SlScreenScraper(IntegrationTestsCredentials.Username, IntegrationTestsCredentials.Password);
            SlScreenScraper cookieAuthScraper;

            // Act
            var authResult = await authScraper.Authenticate();
            cookieAuthScraper = new SlScreenScraper(authScraper.GetCookieHeader());

            var cards = await cookieAuthScraper.GetAccessCards(authResult.PartyRef);

            // Assert
            cards.Should().NotBeNull();

            if (cards != null)
            {
                cards.Count.Should().BeGreaterThan(0);
            }
        }
    }
}
