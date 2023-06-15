using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.Common.Wrappers;

namespace GameTracker.Plugins.Common.Factories
{
    public class HttpClientWrapperFactory : IHttpClientWrapperFactory
    {
        public IHttpClientWrapper BuildHttpClient()
        {
            return new HttpClientWrapper();
        }
    }
}