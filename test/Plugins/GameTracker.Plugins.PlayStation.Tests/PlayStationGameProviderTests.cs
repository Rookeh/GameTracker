using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.PlayStation.Helpers;
using GameTracker.Plugins.PlayStation.Interfaces;
using GameTracker.Plugins.PlayStation.Models.GraphQL.GameLibrary;
using GameTracker.Plugins.PlayStation.Models.GraphQL.StoreInfo;
using NSubstitute.ExceptionExtensions;
using System.Net;
using System.Text.Json;

namespace GameTracker.Plugins.PlayStation.Tests
{
    public class PlayStationGameProviderTests
    {
        private readonly IAuthenticationHelper _mockAuthenticationHelper;
        private readonly IHttpClientWrapper _mockHttpClient;

        private readonly PlayStationGameProvider _provider;

        public PlayStationGameProviderTests()
        {
            _mockAuthenticationHelper = Substitute.For<IAuthenticationHelper>();
            _mockHttpClient = Substitute.For<IHttpClientWrapper>();

            var mockHttpClientFactory = Substitute.For<IHttpClientWrapperFactory>();
            mockHttpClientFactory.BuildHttpClient()
                .Returns(_mockHttpClient);

            _provider = new PlayStationGameProvider(_mockAuthenticationHelper, mockHttpClientFactory);
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

            _mockAuthenticationHelper.ExchangeNpssoForCode(npsso)
                .Returns(code);

            _mockAuthenticationHelper.ExchangeCodeForToken(code)
                .Returns(token);

            _mockHttpClient.SendAsync(Arg.Is<HttpRequestMessage>(r => r.Headers.Authorization.Parameter == token))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(response))
                });

            _mockHttpClient.SendAsync(Arg.Is<HttpRequestMessage>(r => r.RequestUri.AbsoluteUri.Contains(Constants.GraphQL.GetStoreDetailsOperation)))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(storeResponse))
                });

            // Act
            await _provider.Refresh(userId, npsso, includeNonGameTitles);

            // Assert
            Assert.Equal(includeNonGameTitles ? 2 : 1, _provider.Games.Count());
            Assert.Equal(imageUrl, _provider.Games.First().Image.Url);
            Assert.Equal(lastPlayed, _provider.Games.First().LastPlayed);
            Assert.Equal(conceptId, _provider.Games.First().PlatformId);
            Assert.Equal(fullPlatform, _provider.Games.First().Platforms.FirstOrDefault()?.Name);
            Assert.Equal(title, _provider.Games.First().Title);
            await _mockAuthenticationHelper.Received(1).ExchangeNpssoForCode(Arg.Any<string>());
            await _mockAuthenticationHelper.Received(1).ExchangeCodeForToken(Arg.Any<string>());
            await _mockHttpClient.Received(2).SendAsync(Arg.Any<HttpRequestMessage>());
        }

        [Fact]
        public async Task Refresh_IfAPICallFails_Throws()
        {
            // Arrange
            var userId = "test";
            var npsso = "abc123";
            var code = "def456";
            var token = "ghi789";

            _mockAuthenticationHelper.ExchangeNpssoForCode(npsso)
                .Returns(code);

            _mockAuthenticationHelper.ExchangeCodeForToken(code)
                .Returns(token);

            _mockHttpClient.SendAsync(Arg.Is<HttpRequestMessage>(r => r.Headers.Authorization.Parameter == token))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _provider.Refresh(userId, npsso, false));
            await _mockHttpClient.Received(1).SendAsync(Arg.Any<HttpRequestMessage>());
        }

        [Fact]
        public async Task Refresh_IfTokenExchangeFails_Throws()
        {
            // Arrange
            var userId = "test";
            var npsso = "abc123";
            var code = "def456";

            _mockAuthenticationHelper.ExchangeNpssoForCode(npsso)
                .Returns(code);

            _mockAuthenticationHelper.ExchangeCodeForToken(code)
                .ThrowsAsync(new ApplicationException());

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _provider.Refresh(userId, npsso, false));            
            await _mockAuthenticationHelper.Received(1).ExchangeNpssoForCode(Arg.Any<string>());
            await _mockAuthenticationHelper.Received(1).ExchangeCodeForToken(Arg.Any<string>());
            await _mockHttpClient.Received(0).SendAsync(Arg.Any<HttpRequestMessage>());
        }

        [Fact]
        public async Task Refresh_IfCodeExchangeFails_Throws()
        {
            // Arrange
            var userId = "test";
            var npsso = "abc123";

            _mockAuthenticationHelper.ExchangeNpssoForCode(npsso)
                .ThrowsAsync(new ApplicationException());

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _provider.Refresh(userId, npsso, false));
            
            await _mockAuthenticationHelper.Received(0).ExchangeCodeForToken(Arg.Any<string>());
            await _mockHttpClient.Received(0).SendAsync(Arg.Any<HttpRequestMessage>());
        }
    }
}