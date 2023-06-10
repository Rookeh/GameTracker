using GameTracker.Interfaces;
using GameTracker.Models;

namespace GameTracker.Plugins.Dummy
{
    public class DummyGameProvider : IGameProvider
    {
        private IEnumerable<Game> _games = Enumerable.Empty<DummyGame>();
        private bool _initialized = false;

        public Platform Platform => new Platform 
        { 
            Name = "Dummy Provider",
            Description = "A dummy game provider used as an example of how to construct a provider plugin.",
        };

        public IEnumerable<Game> Games => _games;

        public Dictionary<string, Type> RequiredParameters => new()
        {
            { "Number of dummy games", typeof(int) }
        };

        public Guid ProviderId => new Guid("105B6030-9F6D-4FE8-924F-027C0ED0078B");

        public bool Initialized => _initialized;

        public async Task Load(ParameterCache parameterCache)
        {
            await Refresh(parameterCache.UserId, parameterCache.Parameters);
        }

        public async Task<ParameterCache> Refresh(string userId, params object[] providerSpecificParameters)
        {
            if (providerSpecificParameters.Length != 1 || !(providerSpecificParameters[0] is int))
            {
                throw new ArgumentException("Number of dummy games must be provided.");
            }

            var numberOfDummyGames = (int)providerSpecificParameters[0];
            _games = Enumerable.Repeat(new DummyGame(), numberOfDummyGames).ToArray();
            await Task.Delay(TimeSpan.FromSeconds(1));

            _initialized = true;

            return new ParameterCache
            {
                Parameters = providerSpecificParameters,
                ProviderId = ProviderId,
                UserId = userId
            };
        }
    }
}