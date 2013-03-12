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
        public async Task AuthenticationShouldReturnTrueWithCorrectCredentials()
        {
            if ("Test" == ConfigurationManager.AppSettings["Environment"])
            {
                // Ignore integration tests on appharbour
                return;
            }

            // Arrange
            var proxy = new SlScreenScraper(IntegrationTestsCredentials.Username, IntegrationTestsCredentials.Password);

            // Act
            var wasSucessful = await proxy.Authenticate();

            // Assert
            wasSucessful.Should().BeTrue();
            proxy.GetCookieHeader().Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public async Task AuthenticationShouldReturnFalseWithInvalidCredentials()
        {
            if ("Test" == ConfigurationManager.AppSettings["Environment"])
            {
                // Ignore integration tests on appharbour
                return;
            }

            // Arrange
            var proxy = new SlScreenScraper("fakeusername", "fakepassword");

            // Act
            var wasSucessful = await proxy.Authenticate();

            // Assert
            wasSucessful.Should().BeFalse();
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
            var isAuthenticated = await proxy.Authenticate();
            
            List<AccessCard> cards = null;

            if (isAuthenticated)
            {
                cards = await proxy.GetAccessCards();
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
            await authScraper.Authenticate();
            cookieAuthScraper = new SlScreenScraper(authScraper.GetCookieHeader());

            var cards = await cookieAuthScraper.GetAccessCards();

            // Assert
            cards.Should().NotBeNull();

            if (cards != null)
            {
                cards.Count.Should().BeGreaterThan(0);
            }
        }
    }
}
