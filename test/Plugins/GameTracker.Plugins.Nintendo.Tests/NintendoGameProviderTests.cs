using GameTracker.Models.Enums;
using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.Nintendo.Models;
using GameTracker.Plugins.Nintendo.Models.EU;
using Moq;

namespace GameTracker.Plugins.Nintendo.Tests
{
    public class NintendoGameProviderTests
    {
        private readonly Mock<IHttpClientWrapper> _mockHttpClient;        
        private readonly NintendoGameProvider _provider;

        public NintendoGameProviderTests()
        {
            _mockHttpClient = new Mock<IHttpClientWrapper>();

            var httpClientFactory = new Mock<IHttpClientWrapperFactory>();
            httpClientFactory.Setup(x => x.BuildHttpClient())
                .Returns(_mockHttpClient.Object);

            _provider = new NintendoGameProvider(httpClientFactory.Object);
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

            _mockHttpClient.Setup(x => x.GetFromJsonAndTypeAsync(It.Is<Uri>(u => u.AbsoluteUri.Contains("http://search.nintendo-europe.com")), typeof(EUGameResponseRoot)))
                .ReturnsAsync(euTitle)
                .Verifiable();

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
            Assert.Equal("Game Studio", _provider.Games.First().Studio.Name);
            Assert.Equal(euTitle.Response.Docs[0].Title, _provider.Games.First().Title);
            _mockHttpClient.Verify();
        }

        [Fact]
        public async Task Refresh_GivenEURegionCodeAndTitleNames_IfAPICallFails_ReturnsEmptyCollection()
        {
            // Arrange
            var userName = "Test";
            var regionCode = "EU";
            var titles = "Title";           

            _mockHttpClient.Setup(x => x.GetFromJsonAndTypeAsync(It.Is<Uri>(u => u.AbsoluteUri.Contains("http://search.nintendo-europe.com")), typeof(EUGameResponseRoot)))
                .ReturnsAsync(null)
                .Verifiable();

            // Act
            await _provider.Refresh(userName, regionCode, titles);

            // Assert
            Assert.Empty(_provider.Games);
            _mockHttpClient.Verify();
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

            _mockHttpClient.Setup(x => x.GetStringAsync(It.Is<string>(u => u.Contains("https://www.nintendo.co.jp"))))
                .ReturnsAsync(responseXml)
                .Verifiable();

            // Act
            await _provider.Refresh(userName, regionCode, titles);

            // Assert
            Assert.Single(_provider.Games);
            Assert.IsType<JPNintendoGame>(_provider.Games.First());
            Assert.Equal("https://localhost/test.jpg", _provider.Games.First().Image.Url);
            Assert.Contains("12345", _provider.Games.First().LaunchCommand.Url);
            Assert.Equal(DateTime.Today, _provider.Games.First().ReleaseDate);
            Assert.Equal("Test Studio", _provider.Games.First().StudioName);
            Assert.Equal("Test Title", _provider.Games.First().Title);
            _mockHttpClient.Verify();
        }

        [Fact]
        public async Task Refresh_GivenJPRegionCodeAndTitleNames_IfAPICallFails_ReturnsEmptyCollection()
        {
            // Arrange
            var userName = "Test";
            var regionCode = "JP";
            var titles = "Title";

            _mockHttpClient.Setup(x => x.GetStringAsync(It.Is<string>(u => u.Contains("https://www.nintendo.co.jp"))))
                .ReturnsAsync(string.Empty)
                .Verifiable();

            // Act
            await _provider.Refresh(userName, regionCode, titles);

            // Assert
            Assert.Empty(_provider.Games);
            _mockHttpClient.Verify();
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