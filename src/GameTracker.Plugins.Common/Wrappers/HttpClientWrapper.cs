using GameTracker.Plugins.Common.Interfaces;
using System.Net.Http.Json;

namespace GameTracker.Plugins.Common.Wrappers
{
    public class HttpClientWrapper : HttpClient, IHttpClientWrapper
    {
        public async Task<object> GetFromJsonAndTypeAsync(Uri requestUri, Type type)
        {
            return await this.GetFromJsonAsync(requestUri, type);
        }
    }
}