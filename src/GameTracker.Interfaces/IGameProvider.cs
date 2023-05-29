using GameTracker.Models;

namespace GameTracker.Interfaces
{
    public interface IGameProvider
    {
        public Guid ProviderId { get; }
        public Platform Platform { get; }
        public IEnumerable<Game> Games { get; }
        public Dictionary<string, Type> RequiredParameters { get; }
        public Task Refresh(params object[] providerSpecificParameters);        
    }
}