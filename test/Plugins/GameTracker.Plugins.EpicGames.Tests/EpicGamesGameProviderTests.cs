using EpicGamesStoreNET.Models;
using GameTracker.Plugins.EpicGames.Interfaces;
using GameTracker.Plugins.EpicGames.Tests.Helpers;
using Moq;

namespace GameTracker.Plugins.EpicGames.Tests
{
    public class EpicGamesGameProviderTests
    {
        private readonly Mock<IEpicGamesStore> _mockEpicStore;
        private readonly EpicGamesGameProvider _provider;

        public EpicGamesGameProviderTests()
        {
            _mockEpicStore = new Mock<IEpicGamesStore>();
            _provider = new EpicGamesGameProvider(_mockEpicStore.Object);
        }

        [Fact]
        public async Task Refresh_GivenTitleList_TransformsElementsToEpicGameCollection()
        {
            // Arrange
            var userId = "test";
            var titleToQuery = "title";
            var parameters = new[] { titleToQuery };
            var title = "Test Title";
            var description = "Test Description";
            var publisher = "Test Publisher";
            var developer = "Test Developer";

            var response = BuildTestResponse(title, description, publisher, developer);

            _mockEpicStore.Setup(x => x.SearchAsync(titleToQuery))
                .ReturnsAsync(response)
                .Verifiable();

            // Act
            await _provider.Refresh(userId, parameters);

            // Assert
            _mockEpicStore.Verify();
            Assert.Single(_provider.Games);
            Assert.Equal(title, _provider.Games.First().Title);
            Assert.Equal(description, _provider.Games.First().Description);
            Assert.Equal(publisher, _provider.Games.First().PublisherName);
            Assert.Equal(developer, _provider.Games.First().StudioName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task Refresh_GivenInvalidParameters_ThrowsArgumentException(int? numberOfParams)
        {
            // Arrange
            var userId = "test";
            var invalidParam = 1;
            var parameters = numberOfParams.HasValue 
                ? Enumerable.Repeat(invalidParam, numberOfParams.Value).ToArray() 
                : null;

            // Act / Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _provider.Refresh(userId, parameters));
        }

        [Fact]
        public async Task Refresh_GivenTitleList_IfEpicGamesStoreThrows_ExceptionIsRethrown()
        {
            // Arrange
            var userId = "test";
            var titleToQuery = "title";
            var parameters = new[] { titleToQuery };

            _mockEpicStore.Setup(x => x.SearchAsync(titleToQuery))
                .ThrowsAsync(new Exception())
                .Verifiable();

            // Act / Assert
            await Assert.ThrowsAsync<Exception>(async () => await _provider.Refresh(userId, parameters));

        }

        #region Private methods

        private static Response BuildTestResponse(string title, string description, string publisher, string developer)
        {
            var customAttribute = new CustomAttribute();
            customAttribute.SetProtectedValue("Key", "developerName");
            customAttribute.SetProtectedValue("Value", developer);
            var customAttributeArray = new[] { customAttribute };

            var seller = new Seller();
            seller.SetProtectedValue("Id", "SellerId");
            seller.SetProtectedValue("Name", publisher);

            var keyImage = new KeyImage();
            keyImage.SetProtectedValue("Type", "OfferImageWide");
            keyImage.SetProtectedValue("Url", "http://localhost/test.jpg");
            var keyImageArray = new[] { keyImage };

            var element = new Element();
            element.SetProtectedValue("Id", "AABBCCDDE200");
            element.SetProtectedValue("Description", description);
            element.SetProtectedValue("KeyImages", keyImageArray);
            element.SetProtectedValue("Seller", seller);
            element.SetProtectedValue("EffectiveDate", DateTime.Today);
            element.SetProtectedValue("Title", title);
            element.SetProtectedValue("Namespace", Guid.NewGuid().ToString());
            element.SetProtectedValue("UrlSlug", "http://localhost");
            element.SetProtectedValue("CustomAttributes", customAttributeArray);
            var elementArray = new[] { element };

            var searchStore = new SearchStore();
            searchStore.SetProtectedValue("Elements", elementArray);

            var catalog = new Catalog();
            catalog.SetProtectedValue("SearchStore", searchStore);

            var data = new Data();
            data.SetProtectedValue("Catalog", catalog);

            var response = new Response();
            response.SetProtectedValue("Data", data);

            return response;


            //return new Response()
            //{
            //    Data = new Data
            //    {
            //        Catalog = new Catalog
            //        {
            //            SearchStore = new SearchStore
            //            {
            //                Elements = new[]
            //                {
            //                    new Element
            //                    {
            //                        Description = description,
            //                        KeyImages = new []
            //                        {
            //                            new KeyImage
            //                            {
            //                                Type = "OfferImageWide",
            //                                Url = "http://localhost/test.jpg"
            //                            }
            //                        },
            //                        Seller = new Seller
            //                        {
            //                            Id = "SellerId",
            //                            Name = publisher
            //                        },
            //                        EffectiveDate = DateTime.Today,
            //                        Title = title,
            //                        Namespace = Guid.NewGuid().ToString(),
            //                        UrlSlug = "http://localhost",
            //                        CustomAttributes = new []
            //                        {
            //                            new CustomAttribute
            //                            {
            //                                Key = "developerName",
            //                                Value = developer
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //};
        }

        #endregion
    }
}