using GameTracker.Models.Enums;
using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.Nintendo.Models;
using GameTracker.Plugins.Nintendo.Models.EU;

namespace GameTracker.Plugins.Nintendo.Tests
{
    public class NintendoGameProviderTests
    {
        private readonly IHttpClientWrapper _mockHttpClient;        
        private readonly NintendoGameProvider _provider;

        public NintendoGameProviderTests()
        {
            _mockHttpClient = Substitute.For<IHttpClientWrapper>();

            var httpClientFactory = Substitute.For<IHttpClientWrapperFactory>();
            httpClientFactory.BuildHttpClient()
                .Returns(_mockHttpClient);

            _provider = new NintendoGameProvider(httpClientFactory);
        }

        [Fact]
        public async Task Refresh_GivenEURegionCodeAndTitleNames_IfAPICallSucceeds_ReturnsEUNintendoGameCollection()
        {
            // Arrange
            var userName = "Test";
            var regionCode = "EU";
            var titles = "Title";

            var euTitle = new EUGameResponseRoot
            {
                Response = new EUGameResponse
                {
                    Docs = new[]
                    {
                        new EUDocument
                        {
                            ProductCatalogDescription = "Description",
                            GameCategories = new []
                            {
                                "other"
                            },
                            ImageUrlH2X1 = "http://localhost/test.png",
                            Url = "/12345",
                            DatesReleased = new[]
                            {
                                DateTime.Today
                            },
                            Copyright = "Copyright 2023 Game Studio",
                            Title = "Test Title"                            
                        }
                    }
                }
            };

            _mockHttpClient.GetFromJsonAndTypeAsync(Arg.Is<Uri>(u => u.AbsoluteUri.Contains("http://search.nintendo-europe.com")), typeof(EUGameResponseRoot))
                .Returns(euTitle);

            // Act
            await _provider.Refresh(userName, regionCode, titles);

            // Assert
            Assert.Single(_provider.Games);
            Assert.IsType<EUNintendoGame>(_provider.Games.First());
            Assert.Equal(euTitle.Response.Docs[0].ProductCatalogDescription, _provider.Games.First().Description);
            Assert.Equal(Genre.Other, _provider.Games.FirstOrDefault()?.Genres.FirstOrDefault());
            Assert.Equal(euTitle.Response.Docs[0].ImageUrlH2X1, _provider.Games.First().Image.Url);
            Assert.Contains(euTitle.Response.Docs[0].Url, _provider.Games.First().LaunchCommand.Url);
            Assert.Equal(euTitle.Response.Docs[0].DatesReleased.FirstOrDefault(), _provider.Games.First().ReleaseDate);
            Assert.Equal("Game Studio", _provider.Games.First().Studio);
            Assert.Equal(euTitle.Response.Docs[0].Title, _provider.Games.First().Title);
            await _mockHttpClient.Received(1).GetFromJsonAndTypeAsync(Arg.Any<Uri>(), Arg.Any<Type>());
        }

        [Fact]
        public async Task Refresh_GivenEURegionCodeAndTitleNames_IfAPICallFails_ReturnsEmptyCollection()
        {
            // Arrange
            var userName = "Test";
            var regionCode = "EU";
            var titles = "Title";

            EUGameResponseRoot returnValue = null;
            _mockHttpClient.GetFromJsonAndTypeAsync(Arg.Is<Uri>(u => u.AbsoluteUri.Contains("http://search.nintendo-europe.com")), typeof(EUGameResponseRoot))
                .Returns(returnValue);

            // Act
            await _provider.Refresh(userName, regionCode, titles);

            // Assert
            Assert.Empty(_provider.Games);
            await _mockHttpClient.Received(1).GetFromJsonAndTypeAsync(Arg.Any<Uri>(), Arg.Any<Type>());
        }

        [Fact]
        public async Task Refresh_GivenJPRegionCodeAndTitleNames_IfAPICallSucceeds_ReturnsEUNintendoGameCollection()
        {
            // Arrange
            var userName = "Test";
            var regionCode = "JP";
            var titles = "Title";
            var responseXml = $@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
                                 <TitleInfoList>
                                   <TitleInfo>
                                     <InitialCode>ABCD1234</InitialCode>
                                     <TitleName>Test Title</TitleName>
                                     <MakerName>Test Studio</MakerName>
                                     <MakerKana></MakerKana>
                                     <Price></Price>
                                     <SalesDate>{DateTime.Today.ToString("yyyy.MM.dd")}</SalesDate>
                                     <PlatformID>1</PlatformID>
                                     <LinkURL>/titles/12345</LinkURL>
                                     <ScreenshotImgURL>https://localhost/test.jpg</ScreenshotImgURL>
                                   </TitleInfo>
                                 </TitleInfoList>";

            _mockHttpClient.GetStringAsync(Arg.Is<string>(u => u.Contains("https://www.nintendo.co.jp")))
                .Returns(responseXml);

            // Act
            await _provider.Refresh(userName, regionCode, titles);

            // Assert
            Assert.Single(_provider.Games);
            Assert.IsType<JPNintendoGame>(_provider.Games.First());
            Assert.Equal("https://localhost/test.jpg", _provider.Games.First().Image.Url);
            Assert.Contains("12345", _provider.Games.First().LaunchCommand.Url);
            Assert.Equal(DateTime.Today, _provider.Games.First().ReleaseDate);
            Assert.Equal("Test Studio", _provider.Games.First().Studio);
            Assert.Equal("Test Title", _provider.Games.First().Title);
            await _mockHttpClient.Received().GetStringAsync(Arg.Any<string>());
        }

        [Fact]
        public async Task Refresh_GivenJPRegionCodeAndTitleNames_IfAPICallFails_ReturnsEmptyCollection()
        {
            // Arrange
            var userName = "Test";
            var regionCode = "JP";
            var titles = "Title";

            _mockHttpClient.GetStringAsync(Arg.Is<string>(u => u.Contains("https://www.nintendo.co.jp")))
                .Returns(string.Empty);

            // Act
            await _provider.Refresh(userName, regionCode, titles);

            // Assert
            Assert.Empty(_provider.Games);
            await _mockHttpClient.Received(1).GetStringAsync(Arg.Any<string>());
        }

        [Fact]
        public async Task Refresh_GivenNARegionCode_ThrowsNotSupportedException()
        {
            // Arrange
            var userName = "Test";
            var regionCode = "NA";
            var titles = "Title";

            // Act / Assert
            await Assert.ThrowsAsync<NotSupportedException>(() => _provider.Refresh(userName, regionCode, titles));
        }

        [Fact]
        public async Task Refresh_GivenInvalidRegionCode_ThrowsArgumentException()
        {
            // Arrange
            var userName = "Test";
            var regionCode = "XX";
            var titles = "Title";

            // Act / Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _provider.Refresh(userName, regionCode, titles));
        }
    }
}