using GameTracker.Interfaces;
using GameTracker.Interfaces.Data;
using GameTracker.Models;

namespace GameTracker.Core
{
    public class GameLibraryManager : IGameLibraryManager
    {
        private readonly IParameterCacheRepository _parameterCacheRepository;
        private readonly IProviderFactory _providerFactory;
        private readonly Dictionary<Guid, IEnumerable<Game>> _providerGameDictionary;

        public GameLibraryManager(IParameterCacheRepository parameterCacheRepository, IProviderFactory providerFactory)
        {
            _parameterCacheRepository = parameterCacheRepository;
            _providerFactory = providerFactory;
            _providerGameDictionary = new Dictionary<Guid, IEnumerable<Game>>();
        }

        public IEnumerable<Game> Games => _providerGameDictionary.Values.SelectMany(g => g)
                                            .OrderBy(g => g.Title)
                                            .ToList();

        public async Task RefreshProvider(string userId, Guid providerId, params object[] parameters)
        {
            var provider = _providerFactory.GetProviders().FirstOrDefault(p => p.ProviderId == providerId);
            if (provider != null)
            {
                try
                {
                    var refreshedParams = await provider.Refresh(userId, parameters);
                    _providerGameDictionary[provider.ProviderId] = provider.Games;
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

        public async Task InitialiseProviders(string userId)
        {
            foreach (var provider in _providerFactory.GetProviders())
            {
                if (!provider.Initialized)
                {
                    try
                    {
                        var parameterCache = await _parameterCacheRepository.GetParameters(userId, provider.ProviderId);
                        if (parameterCache != null && parameterCache.Parameters.Length == provider.RequiredParameters.Count())
                        {
                            parameterCache.Parameters = MapParameterTypes(provider.RequiredParameters.Values.ToArray(), parameterCache.Parameters).ToArray();
                            await provider.Load(parameterCache);
                            foreach (var game in provider.Games)
                            {
                                await game.Preload();
                            }

                            _providerGameDictionary[provider.ProviderId] = provider.Games;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

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
    }
}