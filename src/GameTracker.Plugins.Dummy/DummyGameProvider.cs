using GameTracker.Interfaces;
using GameTracker.Models;

namespace GameTracker.Plugins.Dummy
{
    public class DummyGameProvider : IGameProvider
    {
        private IEnumerable<Game> _games = Enumerable.Empty<DummyGame>();

        public Platform Platform => new Platform { Name = "Dummy Provider" };

        public IEnumerable<Game> Games => _games;

        public Dictionary<string, Type> RequiredParameters => new Dictionary<string, Type>();

        public Guid ProviderId => new Guid("105B6030-9F6D-4FE8-924F-027C0ED0078B");

        public async Task Refresh(params object[] providerSpecificParameters)
        {
            _games = Enumerable.Repeat(new DummyGame(), 50).ToArray();
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}