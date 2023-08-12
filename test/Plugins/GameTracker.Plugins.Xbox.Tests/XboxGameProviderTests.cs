using GameTracker.Plugins.Xbox.Interfaces;
using GameTracker.Plugins.Xbox.Models.OpenXBL;
using System.Net.Http.Headers;

namespace GameTracker.Plugins.Xbox.Tests
{
    public class XboxGameProviderTests
    {
        private readonly IRateLimitedXboxHttpClient _mockRateLimitedClient;
        private readonly XboxGameProvider _provider;

        public XboxGameProviderTests()
        {
            _mockRateLimitedClient = Substitute.For<IRateLimitedXboxHttpClient>();
            _provider = new XboxGameProvider(_mockRateLimitedClient);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public async Task Refresh_GivenAPIKey_QueriesAPIForTitles(bool includeGamePass, bool includeLegacy)
        {
            // Arrange
            var userId = "test";
            var apiKey = Guid.NewGuid().ToString();
            var imageUrl = "http://localhost/test.png";
            var title = "Test Title";
            var publisher = "Test Publisher";
            var platform = "Windows";

            var response = new XboxLiveTitleResponse
            {
                Titles = new[]
                {
                    new Title
                    {
                        Devices = new[]
                        {
                            "PC"
                        },
                        DisplayImage = imageUrl,
                        GamePass = new GamePass
                        {
                            IsGamePass = false
                        },
                        TitleHistory = new TitleHistory
                        {
                            LastTimePlayed = DateTime.Today
                        },
                        PFN = "TestPublisher.46304FA5-C95E-4B32-9875-0704C075506A_a1bc2defghijk",
                        Name = title
                    },
                    new Title
                    {
                        Devices = new[]
                        {
                            "Xbox360"
                        },
                        DisplayImage = imageUrl,
                        GamePass = new GamePass
                        {
                            IsGamePass = false
                        },
                        TitleHistory = new TitleHistory
                        {
                            LastTimePlayed = DateTime.Today
                        },
                        PFN = "TestPublisher.46304FA5-C95E-4B32-9875-0704C075506A_a1bc2defghijk",
                        Name = title
                    },
                    new Title
                    {
                        Devices = new[]
                        {
                            "PC"
                        },
                        DisplayImage = imageUrl,
                        GamePass = new GamePass
                        {
                            IsGamePass = true
                        },
                        TitleHistory = new TitleHistory
                        {
                            LastTimePlayed = DateTime.Today
                        },
                        PFN = "TestPublisher.46304FA5-C95E-4B32-9875-0704C075506A_a1bc2defghijk",
                        Name = title
                    }
                }
            };

            _mockRateLimitedClient.GetFromJson(Arg.Any<string>(), Arg.Any<XboxLiveTitleResponse>(), Arg.Any<NameValueHeaderValue[]>())
                .Returns(response);

            // Act
            await _provider.Refresh(userId, apiKey, includeGamePass, includeLegacy);

            // Assert
            var options = (includeGamePass, includeLegacy);
            var expectedCount = options switch
            {
                (false, false) => 1,
                (false, true) => 2,
                (true, false) => 2,
                (true, true) => 3
            };

            Assert.Equal(expectedCount, _provider.Games.Count());
            Assert.Equal(DateTime.Today, _provider.Games.First().LastPlayed);
            Assert.Equal(imageUrl, _provider.Games.First().Image.Url);
            Assert.Equal(publisher, _provider.Games.First().PublisherName);
            Assert.Equal(platform, _provider.Games.First().Platforms.FirstOrDefault()?.Name);
            Assert.Equal(title, _provider.Games.First().Title);
            await _mockRateLimitedClient.Received(1).GetFromJson(Arg.Any<string>(), Arg.Any<XboxLiveTitleResponse>(), Arg.Any<NameValueHeaderValue[]>());
        }

        [Fact]
        public async Task Refresh_IfAPIKeyNotProvided_Throws()
        {
            // Arrange
            var userId = "test";

            // Act / Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _provider.Refresh(userId, Array.Empty<object>()));
        }

        [Fact]
        public async Task Refresh_IfIncludeGamePassNotSpecified_Throws()
        {
            // Arrange
            var userId = "test";
            var apiKey = Guid.NewGuid().ToString();

            // Act / Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _provider.Refresh(userId, apiKey));
        }

        [Fact]
        public async Task Refresh_IfIncludeLegacyNotSpecified_Throws()
        {
            // Arrange
            var userId = "test";
            var apiKey = Guid.NewGuid().ToString();
            var includeGamePass = true;

            // Act / Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _provider.Refresh(userId, apiKey, includeGamePass));
        }
    }
}