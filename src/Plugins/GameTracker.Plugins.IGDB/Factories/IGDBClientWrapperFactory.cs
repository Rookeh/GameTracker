using GameTracker.Plugins.IGDB.Interfaces;
using GameTracker.Plugins.IGDB.Wrappers;
using IGDB;

namespace GameTracker.Plugins.IGDB.Factories
{
    public class IGDBClientWrapperFactory : IIGDBClientWrapperFactory
    {
        public IIGDBClientWrapper BuildIGDBClientWrapper(string clientId, string clientSecret)
        {
            return new IGDBClientWrapper(new IGDBClient(clientId, clientSecret));
        }
    }
}