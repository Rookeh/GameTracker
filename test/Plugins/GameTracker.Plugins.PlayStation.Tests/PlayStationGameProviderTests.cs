using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.PlayStation.Helpers;
using GameTracker.Plugins.PlayStation.Interfaces;
using GameTracker.Plugins.PlayStation.Models.GraphQL;
using GameTracker.Plugins.PlayStation.Models.GraphQL.GameLibrary;
using GameTracker.Plugins.PlayStation.Models.GraphQL.StoreInfo;
using Moq;
using System.Net;
using System.Text.Json;

namespace GameTracker.Plugins.PlayStation.Tests
{
    public class PlayStationGameProviderTests
    {
        private readonly Mock<IAuthenticationHelper> _mockAuthenticationHelper;
        private readonly Mock<IHttpClientWrapper> _mockHttpClient;

        private readonly PlayStationGameProvider _provider;

        public PlayStationGameProviderTests()
        {
            _mockAuthenticationHelper = new Mock<IAuthenticationHelper>();
            _mockHttpClient = new Mock<IHttpClientWrapper>();

            var mockHttpClientFactory = new Mock<IHttpClientWrapperFactory>();
            mockHttpClientFactory.Setup(x => x.BuildHttpClient())
                .Returns(_mockHttpClient.Object);

            _provider = new PlayStationGameProvider(_mockAuthenticationHelper.Object, mockHttpClientFactory.Object);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Refresh_GivenNPSSOToken_QueriesAPIForTitles(bool includeNonGameTitles)
        {
            // Arrange
            var userId = "test";
            var npsso = "abc123";
            var code = "def456";
            var token = "ghi789";

            var imageUrl = "http://localhost/test.png";
            var lastPlayed = DateTime.Today;
            var productId = 1234;
            var conceptId = 5678;
            var platform = "PS5";
            var fullPlatform = "PlayStation 5";
            var title = "Test Title";
            var nonGameTitle = "Non-Game Title";

            var response = new GameResponse
            {
                Data = new Models.GraphQL.GameLibrary.Data
                {
                    GameLibraryTitlesRetrieve = new GameLibraryTitlesRetrieve
                    {
                        Games = new[]
                        {
                            new Game
                            {
                                ConceptId = conceptId.ToString(),
                                Image = new Image
                                {
                                    URL = imageUrl
                                },
                                LastPlayedDateTime = lastPlayed,
                                Name = title,
                                ProductId = productId.ToString(),
                                Platform = platform                   
                            },
                            new Game
                            {
                                Image = new Image
                                {
                                    URL = imageUrl
                                },
                                LastPlayedDateTime = lastPlayed,
                                Platform = platform,
                                Name = nonGameTitle
                            }
                        }
                    }
                }
            };

            var storeResponse = new StoreInfoRoot
            {
                Data = new Models.GraphQL.StoreInfo.Data
                {
                    ProductRetrieve = new ProductRetrieve
                    {
                        LocalizedGenres = new[]
                        {
                            new LocalizedGenre
                            {
                                Value = "Test"
                            }
                        }
                    }
                }
            };

            _mockAuthenticationHelper.Setup(x => x.ExchangeNpssoForCode(npsso))
                .ReturnsAsync(code)
                .Verifiable();

            _mockAuthenticationHelper.Setup(x => x.ExchangeCodeForToken(code))
                .ReturnsAsync(token)
                .Verifiable();

            _mockHttpClient.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(r => r.Headers.Authorization.Parameter == token)))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(response))
                })
                .Verifiable();

            _mockHttpClient.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(r => r.RequestUri.AbsoluteUri.Contains(Constants.GraphQL.GetStoreDetailsOperation))))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(storeResponse))
                })
                .Verifiable();

            // Act
            await _provider.Refresh(userId, npsso, includeNonGameTitles);

            // Assert
            Assert.Equal(includeNonGameTitles ? 2 : 1, _provider.Games.Count());
            Assert.Equal(imageUrl, _provider.Games.First().Image.Url);
            Assert.Equal(lastPlayed, _provider.Games.First().LastPlayed);
            Assert.Equal(conceptId, _provider.Games.First().PlatformId);
            Assert.Equal(fullPlatform, _provider.Games.First().Platforms.FirstOrDefault()?.Name);
            Assert.Equal(title, _provider.Games.First().Title);
            _mockAuthenticationHelper.Verify();
            _mockHttpClient.Verify();
        }

        [Fact]
        public async Task Refresh_IfAPICallFails_Throws()
        {
            // Arrange
            var userId = "test";
            var npsso = "abc123";
            var code = "def456";
            var token = "ghi789";

            _mockAuthenticationHelper.Setup(x => x.ExchangeNpssoForCode(npsso))
                .ReturnsAsync(code)
                .Verifiable();

            _mockAuthenticationHelper.Setup(x => x.ExchangeCodeForToken(code))
                .ReturnsAsync(token)
                .Verifiable();

            _mockHttpClient.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(r => r.Headers.Authorization.Parameter == token)))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                })
                .Verifiable();

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _provider.Refresh(userId, npsso, false));
            _mockHttpClient.Verify();
        }

        [Fact]
        public async Task Refresh_IfTokenExchangeFails_Throws()
        {
            // Arrange
            var userId = "test";
            var npsso = "abc123";
            var code = "def456";

            _mockAuthenticationHelper.Setup(x => x.ExchangeNpssoForCode(npsso))
                .ReturnsAsync(code)
                .Verifiable();

            _mockAuthenticationHelper.Setup(x => x.ExchangeCodeForToken(code))
                .ThrowsAsync(new ApplicationException())
                .Verifiable();

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _provider.Refresh(userId, npsso, false));
            _mockAuthenticationHelper.Verify();
            _mockHttpClient.Verify(x => x.SendAsync(It.IsAny<HttpRequestMessage>()), Times.Never);
        }

        [Fact]
        public async Task Refresh_IfCodeExchangeFails_Throws()
        {
            // Arrange
            var userId = "test";
            var npsso = "abc123";

            _mockAuthenticationHelper.Setup(x => x.ExchangeNpssoForCode(npsso))
                .ThrowsAsync(new ApplicationException())
                .Verifiable();

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _provider.Refresh(userId, npsso, false));
            _mockAuthenticationHelper.Verify();
            _mockAuthenticationHelper.Verify(x => x.ExchangeCodeForToken(It.IsAny<string>()), Times.Never);
            _mockHttpClient.Verify(x => x.SendAsync(It.IsAny<HttpRequestMessage>()), Times.Never);
        }
    }
}
