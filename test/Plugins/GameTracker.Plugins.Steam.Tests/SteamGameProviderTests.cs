using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.Steam.Interfaces.ApiClients;
using GameTracker.Plugins.Steam.Interfaces.Data;
using GameTracker.Plugins.Steam.Models.WebApi;
using Moq;
using System.Net;
using System.Text.Json;

namespace GameTracker.Plugins.Steam.Tests
{
    public class SteamGameProviderTests
    {
        private readonly Mock<IHttpClientWrapper> _mockHttpClient;
        private readonly Mock<IRateLimitedSteamApiClient> _mockSteamApiClient;
        private readonly Mock<ISteamGameDetailsRepository> _mockSteamGameDetailsRepo;

        private readonly SteamGameProvider _provider;

        public SteamGameProviderTests()
        {
            _mockHttpClient = new Mock<IHttpClientWrapper>();
            _mockSteamApiClient = new Mock<IRateLimitedSteamApiClient>();
            _mockSteamGameDetailsRepo = new Mock<ISteamGameDetailsRepository>();

            var mockHttpClientWrapperFactory = new Mock<IHttpClientWrapperFactory>();
            mockHttpClientWrapperFactory.Setup(x => x.BuildHttpClient())
                .Returns(_mockHttpClient.Object);

            _provider = new SteamGameProvider(mockHttpClientWrapperFactory.Object,
                _mockSteamApiClient.Object,
                _mockSteamGameDetailsRepo.Object);
        }

        [Fact]
        public async Task Refresh_GivenSteamApiKeyAndSteamID64_QueriesAPIForOwnedTitles()
        {
            // Arrange
            var userId = "test";
            var apiKey = "abcd";
            var steamId = "1234";

            var appId = 1;
            var lastPlayed = (long)(DateTime.Today - DateTime.UnixEpoch).TotalSeconds;
            var playTime = TimeSpan.FromHours(1);
            var title = "Test Title";

            var ownedGamesResponse = new SteamGameResponseRoot
            {
                Response = new SteamGameResponse
                {
                    GameCount = 1,
                    Games = new[]
                    {
                        new SteamGameDto
                        {
                            AppId = appId,
                            LastPlayedTimestamp = lastPlayed,
                            Playtime = (int)playTime.TotalMinutes,
                            Name = title
                        }
                    }
                }
            };

            _mockHttpClient.Setup(x => x.GetAsync(It.Is<Uri>(u => u.AbsoluteUri.Contains(apiKey) && u.AbsoluteUri.Contains(steamId))))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(ownedGamesResponse))
                })
                .Verifiable();

            // Act
            await _provider.Refresh(userId, apiKey, steamId);

            // Assert
            Assert.Single(_provider.Games);
            Assert.Equal(appId, _provider.Games.First().PlatformId);
            Assert.Equal(title, _provider.Games.First().Title);
            Assert.Equal(DateTime.Today, _provider.Games.First().LastPlayed);
            Assert.Equal(playTime, _provider.Games.First().Playtime);
            _mockHttpClient.Verify();
        }

        [Fact]
        public async Task Refresh_IfAPIKeyIsRejected_ThrowsApplicationException()
        {
            // Arrange
            var userId = "test";
            var apiKey = "abcd";
            var steamId = "1234";

            _mockHttpClient.Setup(x => x.GetAsync(It.Is<Uri>(u => u.AbsoluteUri.Contains(apiKey) && u.AbsoluteUri.Contains(steamId))))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized
                })
                .Verifiable();

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _provider.Refresh(userId, apiKey, steamId));
            _mockHttpClient.Verify();
        }
    }
}