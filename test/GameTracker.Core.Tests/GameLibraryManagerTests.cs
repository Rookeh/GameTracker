using GameTracker.Interfaces;
using GameTracker.Interfaces.Data;
using GameTracker.Models;

namespace GameTracker.Core.Tests
{
    public class GameLibraryManagerTests
    {
        private readonly IGameProvider _mockGameProvider;
        private readonly IParameterCacheRepository _mockParameterCacheRepo;
        private readonly IGameLibraryManager _gameLibraryManager;

        public GameLibraryManagerTests()
        {
            _mockGameProvider = Substitute.For<IGameProvider>();
            _mockParameterCacheRepo = Substitute.For<IParameterCacheRepository>();

            var gameProviderFactory = Substitute.For<IGameProviderFactory>();
            gameProviderFactory.GetProviders().Returns(new[] { _mockGameProvider });

            _gameLibraryManager = new GameLibraryManager(_mockParameterCacheRepo, gameProviderFactory);
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

            _mockGameProvider.Initialized.Returns(false);
            _mockGameProvider.ProviderId.Returns(providerGuid);
            _mockGameProvider.RequiredParameters.Returns(new Dictionary<string, Type>
            {
                ["test"] = typeof(string)
            });

            _mockParameterCacheRepo.GetParameters(userId, providerGuid)
                .Returns(cachedParams);

            // Act
            await _gameLibraryManager.InitialiseProviders(userId);

            // Assert            
            _ = _mockGameProvider.Received().Initialized;
            _ = _mockGameProvider.Received().ProviderId;
            _ = _mockGameProvider.Received().RequiredParameters;
            await _mockParameterCacheRepo.Received().GetParameters(userId, providerGuid);
            await _mockGameProvider.Received(1).Load(Arg.Any<ParameterCache>());
            await _mockGameProvider.Received().Load(cachedParams);
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

            var mockGame = Substitute.For<Game>();

            _mockGameProvider.Initialized.Returns(false);
            _mockGameProvider.ProviderId.Returns(providerGuid);
            _mockGameProvider.RequiredParameters
                .Returns(new Dictionary<string, Type>
                {
                    ["test"] = typeof(string)
                });
            _mockGameProvider.Games
                .Returns(new[] { mockGame });
            _mockParameterCacheRepo.GetParameters(userId, providerGuid)
                .Returns(cachedParams);

            // Act
            await _gameLibraryManager.InitialiseProviders(userId);

            // Assert
            await mockGame.Received(1).Preload();
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

            _mockGameProvider.ProviderId.Returns(providerGuid);
            _mockGameProvider.Refresh(userId, paramValues)
                .Returns(parameters);
            _mockParameterCacheRepo.GetParameters(userId, providerGuid)
                .Returns((ParameterCache?)null);

            // Act
            await _gameLibraryManager.RefreshProvider(userId, providerGuid, paramValues);

            // Assert
            await _mockParameterCacheRepo.Received(1).InsertParameters(Arg.Any<ParameterCache>());
            await _mockParameterCacheRepo.Received().InsertParameters(parameters);
            await _mockParameterCacheRepo.Received(0).UpdateParameters(Arg.Any<ParameterCache>());
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

            _mockGameProvider.ProviderId.Returns(providerGuid);
            _mockGameProvider.Refresh(userId, paramValues)
                .Returns(parameters);
            _mockParameterCacheRepo.GetParameters(userId, providerGuid)
                .Returns(new ParameterCache { Parameters = paramValues });

            // Act
            await _gameLibraryManager.RefreshProvider(userId, providerGuid, paramValues);

            // Assert
            await _mockParameterCacheRepo.Received(1).UpdateParameters(Arg.Any<ParameterCache>());
            await _mockParameterCacheRepo.Received().UpdateParameters(parameters);
            await _mockParameterCacheRepo.Received(0).InsertParameters(Arg.Any<ParameterCache>());
        }
    }
}