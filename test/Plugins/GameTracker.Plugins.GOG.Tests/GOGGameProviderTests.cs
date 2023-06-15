using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.GOG.Helpers;
using GameTracker.Plugins.GOG.Interfaces;
using GameTracker.Plugins.GOG.Models.GOGApi;
using Moq;
using System.Net;
using System.Text.Json;

namespace GameTracker.Plugins.GOG.Tests
{
    public class GOGGameProviderTests
    {
        private readonly Mock<IAuthenticationHelper> _mockAuthHelper;
        private readonly Mock<IHttpClientWrapper> _mockHttpClient;

        private readonly GOGGameProvider _provider;

        public GOGGameProviderTests()
        {
            _mockAuthHelper = new Mock<IAuthenticationHelper>();
            _mockHttpClient = new Mock<IHttpClientWrapper>();

            var httpClientFactory = new Mock<IHttpClientWrapperFactory>();
            httpClientFactory.Setup(x => x.BuildHttpClient())
                .Returns(_mockHttpClient.Object);

            _provider = new GOGGameProvider(_mockAuthHelper.Object, httpClientFactory.Object);
        }

        [Fact]
        public async Task Refresh_GivenValidGOGAuthCode_TransformsGOGApiResponseToGOGGameCollection()
        {
            // Arrange
            var userId = "test";
            var gogCode = "abcd1234";
            var authToken = new AuthToken
            {
                AccessToken = "defg5678"
            };

            var ownedGames = new OwnedGames
            {
                Owned = new[] { 1 }
            };

            var gameDetails = new GameDetails
            {               
                ContentSystemCompatibility = new ContentSystemCompatibility
                {
                    Windows = true,
                    Linux = true,
                    OSX = true
                },
                Description = new Description
                {
                    Full = "Test Description"
                },
                Id = 1,
                Images = new Images
                {
                    Logo2x = "http://localhost/test.jpg"
                },
                ReleaseDate = DateTime.Today.ToString(),
                Title = "Test Title"
            };

            _mockAuthHelper.Setup(x => x.ExchangeGogAuthCodeForToken(gogCode))
                .ReturnsAsync(authToken)
                .Verifiable();

            _mockHttpClient.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(r => r.RequestUri.ToString() == Constants.Requests.OwnedGamesRequestUrl)))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonSerializer.Serialize(ownedGames)) })
                .Verifiable();

            _mockHttpClient.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(r => r.RequestUri.ToString().Contains("https://api.gog.com/products?ids="))))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonSerializer.Serialize(new[] { gameDetails })) })
                .Verifiable();

            // Act 
            await _provider.Refresh(userId, gogCode);

            // Assert
            Assert.Single(_provider.Games);
            Assert.Equal(gameDetails.Title, _provider.Games.First().Title);
            Assert.Equal(gameDetails.Description.Full, _provider.Games.First().Description);
            Assert.Equal(gameDetails.Id, _provider.Games.First().PlatformId);
            Assert.Equal(gameDetails.Images.Logo2x, _provider.Games.First().Image.Url);
            Assert.Equal(gameDetails.ReleaseDate, _provider.Games.First().ReleaseDate.ToString());
            Assert.Equal(3, _provider.Games.First().Platforms.Count());
            _mockAuthHelper.Verify();
            _mockHttpClient.Verify();
        }

        [Fact]
        public async Task Refresh_GivenValidGOGAuthCode_IfGOGApiFailsForGameDetails_Throws()
        {
            // Arrange
            var userId = "test";
            var gogCode = "abcd1234";
            var authToken = new AuthToken
            {
                AccessToken = "defg5678"
            };

            var ownedGames = new OwnedGames
            {
                Owned = new[] { 1, 2, 3 }
            };

            _mockAuthHelper.Setup(x => x.ExchangeGogAuthCodeForToken(gogCode))
                .ReturnsAsync(authToken)
                .Verifiable();

            _mockHttpClient.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(r => r.RequestUri.ToString() == Constants.Requests.OwnedGamesRequestUrl)))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonSerializer.Serialize(ownedGames)) })
                .Verifiable();

            _mockHttpClient.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(r => r.RequestUri.ToString().Contains("https://api.gog.com/products?ids="))))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError })
                .Verifiable();

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _provider.Refresh(userId, gogCode));
            _mockAuthHelper.Verify();
            _mockHttpClient.Verify();
        }

        [Fact]
        public async Task Refresh_GivenValidGOGAuthCode_IfGOGApiFailsForOwnedGames_Throws()
        {
            // Arrange
            var userId = "test";
            var gogCode = "abcd1234";
            var authToken = new AuthToken
            {
                AccessToken = "defg5678"
            };

            _mockAuthHelper.Setup(x => x.ExchangeGogAuthCodeForToken(gogCode))
                .ReturnsAsync(authToken)
                .Verifiable();

            _mockHttpClient.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(r => r.RequestUri.ToString() == Constants.Requests.OwnedGamesRequestUrl)))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError })
                .Verifiable();

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _provider.Refresh(userId, gogCode));
            _mockAuthHelper.Verify();
            _mockHttpClient.Verify();
        }

        [Fact]
        public async Task Refresh_GivenGOGAuthCode_IfAuthenticationThrows_Exception()
        {
            // Arrange
            var userId = "test";
            var gogCode = "abcd1234";

            _mockAuthHelper.Setup(x => x.ExchangeGogAuthCodeForToken(gogCode))
                .ThrowsAsync(new Exception())
                .Verifiable();

            // Act / Assert
            await Assert.ThrowsAsync<Exception>(() => _provider.Refresh(userId, gogCode));
            _mockAuthHelper.Verify();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task Refresh_GivenInvalidParameters_ThrowsArgumentException(int? numberOfParams)
        {
            // Arrange
            var userId = "test";
            var parameters = numberOfParams.HasValue
                ? Enumerable.Repeat(0, numberOfParams.Value).ToArray()
                : null;

            // Act / Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _provider.Refresh(userId, parameters));
        }
    }
}