using GameTracker.Interfaces;
using GameTracker.Interfaces.Data;
using GameTracker.Models;
using Moq;

namespace GameTracker.Core.Tests
{
    public class GameLibraryManagerTests
    {
        private readonly Mock<IGameProvider> _mockGameProvider;
        private readonly Mock<IParameterCacheRepository> _mockParameterCacheRepo;
        private readonly IGameLibraryManager _gameLibraryManager;

        public GameLibraryManagerTests()
        {
            _mockGameProvider = new Mock<IGameProvider>();
            _mockParameterCacheRepo = new Mock<IParameterCacheRepository>();

            var gameProviderFactory = new Mock<IGameProviderFactory>();
            gameProviderFactory.Setup(x => x.GetProviders())
                .Returns(new[] { _mockGameProvider.Object });

            _gameLibraryManager = new GameLibraryManager(_mockParameterCacheRepo.Object, gameProviderFactory.Object);
        }

        [Fact]
        public async Task InitialiseProviders_IfProviderIsNotInitialised_LoadsProviderWithCachedParameters()
        {
            // Arrange
            string userId = "test";
            string paramValue = "foo";
            var providerGuid = Guid.NewGuid();
            var cachedParams = new ParameterCache
            {
                Parameters = new[] { paramValue },
                ProviderId = providerGuid,
                UserId = userId
            };

            _mockGameProvider.Setup(x => x.Initialized).Returns(false).Verifiable();
            _mockGameProvider.SetupGet(x => x.ProviderId).Returns(providerGuid).Verifiable();
            _mockGameProvider.SetupGet(x => x.RequiredParameters)
                .Returns(new Dictionary<string, Type>
                {
                    ["test"] = typeof(string)
                })
                .Verifiable();

            _mockParameterCacheRepo.Setup(x => x.GetParameters(userId, providerGuid))
                .ReturnsAsync(cachedParams)
                .Verifiable();

            // Act
            await _gameLibraryManager.InitialiseProviders(userId);

            // Assert
            _mockParameterCacheRepo.Verify();
            _mockGameProvider.Verify();
            _mockGameProvider.Verify(x => x.Load(It.IsAny<ParameterCache>()), Times.Once());
            _mockGameProvider.Verify(x => x.Load(cachedParams));
        }

        [Fact]
        public async Task InitialiseProviders_IfProviderIsNotInitialised_CallsPreloadOnProviderGames()
        {
            // Arrange
            string userId = "test";
            string paramValue = "foo";
            var providerGuid = Guid.NewGuid();
            var cachedParams = new ParameterCache
            {
                Parameters = new[] { paramValue },
                ProviderId = providerGuid,
                UserId = userId
            };

            var mockGame = new Mock<Game>();

            _mockGameProvider.Setup(x => x.Initialized).Returns(false);
            _mockGameProvider.SetupGet(x => x.ProviderId).Returns(providerGuid);
            _mockGameProvider.SetupGet(x => x.RequiredParameters)
                .Returns(new Dictionary<string, Type>
                {
                    ["test"] = typeof(string)
                });
            _mockGameProvider.SetupGet(x => x.Games)
                .Returns(new[] { mockGame.Object })
                .Verifiable();

            _mockParameterCacheRepo.Setup(x => x.GetParameters(userId, providerGuid))
                .ReturnsAsync(cachedParams);

            // Act
            await _gameLibraryManager.InitialiseProviders(userId);

            // Assert
            _mockGameProvider.Verify();
            mockGame.Verify(x => x.Preload(), Times.Once);
        }

        [Fact]
        public async Task RefreshProvider_IfNoCachedParameters_InsertNewCachedParameters()
        {
            // Arrange
            string userId = "test";
            var providerGuid = Guid.NewGuid();
            var paramValues = new[] { "foo" };            
            var parameters = new ParameterCache
            {
                Parameters = paramValues,
                ProviderId = providerGuid,
                UserId = userId
            };

            _mockGameProvider.SetupGet(x => x.ProviderId).Returns(providerGuid);
            _mockGameProvider.Setup(x => x.Refresh(userId, paramValues))
                .ReturnsAsync(parameters)
                .Verifiable();
            _mockParameterCacheRepo.Setup(x => x.GetParameters(userId, providerGuid))
                .ReturnsAsync((ParameterCache?)null)
                .Verifiable();

            // Act
            await _gameLibraryManager.RefreshProvider(userId, providerGuid, paramValues);

            // Assert
            _mockGameProvider.Verify();
            _mockParameterCacheRepo.Verify();
            _mockParameterCacheRepo.Verify(x => x.InsertParameters(It.IsAny<ParameterCache>()), Times.Once);
            _mockParameterCacheRepo.Verify(x => x.InsertParameters(parameters));
            _mockParameterCacheRepo.Verify(x => x.UpdateParameters(It.IsAny<ParameterCache>()), Times.Never);
        }

        [Fact]
        public async Task RefreshProvider_IfExistingCachedParameters_UpdateCachedParameters()
        {
            // Arrange
            string userId = "test";
            var providerGuid = Guid.NewGuid();
            var paramValues = new[] { "foo" };
            var parameters = new ParameterCache
            {
                Parameters = paramValues,
                ProviderId = providerGuid,
                UserId = userId
            };

            _mockGameProvider.SetupGet(x => x.ProviderId).Returns(providerGuid);
            _mockGameProvider.Setup(x => x.Refresh(userId, paramValues))
                .ReturnsAsync(parameters)
                .Verifiable();
            _mockParameterCacheRepo.Setup(x => x.GetParameters(userId, providerGuid))
                .ReturnsAsync(new ParameterCache { Parameters = paramValues })
                .Verifiable();

            // Act
            await _gameLibraryManager.RefreshProvider(userId, providerGuid, paramValues);

            // Assert
            _mockGameProvider.Verify();
            _mockParameterCacheRepo.Verify();
            _mockParameterCacheRepo.Verify(x => x.UpdateParameters(It.IsAny<ParameterCache>()), Times.Once);
            _mockParameterCacheRepo.Verify(x => x.UpdateParameters(parameters));
            _mockParameterCacheRepo.Verify(x => x.InsertParameters(It.IsAny<ParameterCache>()), Times.Never);
        }
    }
}