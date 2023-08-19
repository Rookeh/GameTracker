using GameTracker.Models;

namespace GameTracker.Interfaces
{
    public interface IDataProvider
    {
        public IEnumerable<Game> Games { get; }
        public bool Initialized { get; }
        public Task Load(ParameterCache parameterCache);
        public DataPlatform Platform { get; }
        public Guid ProviderId { get; }        
        public Task<ParameterCache> Refresh(string userName, params object[] providerSpecificParameters);
        public Dictionary<string, Type> RequiredParameters { get; }        
    }
}