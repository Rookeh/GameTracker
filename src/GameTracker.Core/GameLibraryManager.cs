using GameTracker.Interfaces;
using GameTracker.Interfaces.Data;
using GameTracker.Models;

namespace GameTracker.Core
{
    public class GameLibraryManager : IGameLibraryManager
    {
        private readonly IParameterCacheRepository _parameterCacheRepository;
        private readonly IDataProviderFactory _providerFactory;
        private readonly Dictionary<Guid, IEnumerable<Game>> _providerGameDictionary;

        public GameLibraryManager(IParameterCacheRepository parameterCacheRepository, IDataProviderFactory providerFactory)
        {
            _parameterCacheRepository = parameterCacheRepository;
            _providerFactory = providerFactory;
            _providerGameDictionary = new Dictionary<Guid, IEnumerable<Game>>();
        }

        public IEnumerable<Game> Games => _providerGameDictionary.Values.SelectMany(g => g)
                                            .OrderBy(g => g.Title)
                                            .ToList();

        public IEnumerable<IGrouping<string, Game>> GamesGroupedByTitle => Games.GroupBy(g => g.Title);        

        public async Task InitialiseProviders(string userId)
        {
            var providers = _providerFactory.GetAllProviders();
            var gameProviders = providers.Where(p => p is IGameProvider)
                .Select(p => p as IGameProvider)
                .ToList();
            var reviewProviders = providers.Where(p => p is IReviewProvider)
                .Select(p => p as IReviewProvider)
                .ToList();

            foreach (var gameProvider in gameProviders)
            {
                if (!gameProvider.Initialized)
                {
                    try
                    {
                        var parameterCache = await _parameterCacheRepository.GetParameters(userId, gameProvider.ProviderId);
                        if (parameterCache != null && parameterCache.Parameters.Length == gameProvider.RequiredParameters.Count())
                        {
                            parameterCache.Parameters = MapParameterTypes(gameProvider.RequiredParameters.Values.ToArray(), parameterCache.Parameters).ToArray();
                            await gameProvider.Load(parameterCache);
                            foreach (var game in gameProvider.Games)
                            {
                                await game.Preload();
                            }

                            _providerGameDictionary[gameProvider.ProviderId] = gameProvider.Games;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            foreach(var reviewProvider in reviewProviders)
            {
                if (!reviewProvider.Initialized)
                {
                    var parameterCache = await _parameterCacheRepository.GetParameters(userId, reviewProvider.ProviderId);
                    if (parameterCache != null && parameterCache.Parameters.Length == reviewProvider.RequiredParameters.Count())
                    {
                        parameterCache.Parameters = MapParameterTypes(reviewProvider.RequiredParameters.Values.ToArray(), parameterCache.Parameters).ToArray();
                        reviewProvider.Games = Games;
                        await reviewProvider.Load(parameterCache);
                    }
                }
            }
        }

        public int InitialisedProviders => _providerGameDictionary.Where(p => p.Value.Any()).Count();

        public async Task RefreshProvider(string userId, Guid providerId, params object[] parameters)
        {
            var provider = _providerFactory.GetAllProviders().FirstOrDefault(p => p.ProviderId == providerId);
            if (provider != null)
            {
                try
                {
                    if (provider is IReviewProvider)
                    {
                        (provider as IReviewProvider).Games = Games;
                    }

                    var refreshedParams = await provider.Refresh(userId, parameters);

                    if (provider is IGameProvider)
                    {
                        _providerGameDictionary[provider.ProviderId] = provider.Games;
                    }
                    
                    var existingParams = await _parameterCacheRepository.GetParameters(userId, providerId);

                    if (existingParams == null || !existingParams.Parameters.Any())
                    {
                        await _parameterCacheRepository.InsertParameters(refreshedParams);
                    }
                    else
                    {
                        await _parameterCacheRepository.UpdateParameters(refreshedParams);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        #region Private methods

        private static IEnumerable<object> MapParameterTypes(Type[] types, object[] values)
        {
            if (types.Count() != values.Count())
            {
                throw new ArgumentException("Parameter mismatch");
            }

            for (int i = 0; i < types.Count(); i++)
            {
                yield return Convert.ChangeType(values[i], types[i]);
            }
        }

        #endregion
    }
}