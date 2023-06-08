using GameTracker.Models;

namespace GameTracker.Interfaces
{
    public interface IGameProvider
    {
        public Guid ProviderId { get; }
        public Platform Platform { get; }
        public IEnumerable<Game> Games { get; }
        public Dictionary<string, Type> RequiredParameters { get; }
        public bool Initialized { get; }
        public Task Load(ParameterCache parameterCache);
        public Task<ParameterCache> Refresh(string userName, params object[] providerSpecificParameters);        
    }
}