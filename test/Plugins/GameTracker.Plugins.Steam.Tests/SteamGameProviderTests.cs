using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.Steam.Interfaces.ApiClients;
using GameTracker.Plugins.Steam.Interfaces.Data;
using GameTracker.Plugins.Steam.Models.WebApi;
using System.Net;
using System.Text.Json;

namespace GameTracker.Plugins.Steam.Tests
{
    public class SteamGameProviderTests
    {
        private readonly IHttpClientWrapper _mockHttpClient;
        private readonly IRateLimitedSteamApiClient _mockSteamApiClient;
        private readonly ISteamGameDetailsRepository _mockSteamGameDetailsRepo;

        private readonly SteamGameProvider _provider;

        public SteamGameProviderTests()
        {
            _mockHttpClient = Substitute.For<IHttpClientWrapper>();
            _mockSteamApiClient = Substitute.For<IRateLimitedSteamApiClient>();
            _mockSteamGameDetailsRepo = Substitute.For<ISteamGameDetailsRepository>();

            var mockHttpClientWrapperFactory = Substitute.For<IHttpClientWrapperFactory>();
            mockHttpClientWrapperFactory.BuildHttpClient()
                .Returns(_mockHttpClient);

            _provider = new SteamGameProvider(mockHttpClientWrapperFactory, _mockSteamApiClient,
                _mockSteamGameDetailsRepo);
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

            _mockHttpClient.GetAsync(Arg.Is<Uri>(u => u.AbsoluteUri.Contains(apiKey) && u.AbsoluteUri.Contains(steamId)))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(ownedGamesResponse))
                });

            // Act
            await _provider.Refresh(userId, apiKey, steamId, true, true);

            // Assert
            Assert.Single(_provider.Games);
            Assert.Equal(appId, _provider.Games.First().PlatformId);
            Assert.Equal(title, _provider.Games.First().Title);
            Assert.Equal(DateTime.Today, _provider.Games.First().LastPlayed);
            Assert.Equal(playTime, _provider.Games.First().Playtime);
            await _mockHttpClient.Received(1).GetAsync(Arg.Any<Uri>());
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public async Task Refresh_RespectsExcludedTitles(bool includeTools, bool includeBetas)
        {
            // Arrange
            var userId = "test";
            var apiKey = "abcd";
            var steamId = "1234";
            var ownedGamesResponse = new SteamGameResponseRoot
            {
                Response = new SteamGameResponse
                {
                    GameCount = 1,
                    Games = new[]
                    {
                        new SteamGameDto
                        {
                            Name = "Test Title",
                        },
                        new SteamGameDto
                        {
                            Name = "Test Dedicated Server"
                        },
                        new SteamGameDto
                        {
                            Name = "Test Driver Updater"
                        },
                        new SteamGameDto
                        {
                            Name = "Test Beta"
                        }
                    }
                }
            };

            _mockHttpClient.GetAsync(Arg.Is<Uri>(u => u.AbsoluteUri.Contains(apiKey) && u.AbsoluteUri.Contains(steamId)))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(ownedGamesResponse))
                });

            // Act
            await _provider.Refresh(userId, apiKey, steamId, includeBetas, includeTools);

            // Assert
            int expectedCount = (includeTools, includeBetas) switch
            {
                (false, false) => 1,
                (false, true) => 2,
                (true, false) => 3,
                (true, true) => 4,
            };

            Assert.Equal(expectedCount, _provider.Games.Count());
            await _mockHttpClient.Received(1).GetAsync(Arg.Any<Uri>());
        }

        [Fact]
        public async Task Refresh_IfAPIKeyIsRejected_ThrowsApplicationException()
        {
            // Arrange
            var userId = "test";
            var apiKey = "abcd";
            var steamId = "1234";

            _mockHttpClient.GetAsync(Arg.Is<Uri>(u => u.AbsoluteUri.Contains(apiKey) && u.AbsoluteUri.Contains(steamId)))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized
                });

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _provider.Refresh(userId, apiKey, steamId, true, true));
            await _mockHttpClient.Received(1).GetAsync(Arg.Any<Uri>());
        }
    }
}