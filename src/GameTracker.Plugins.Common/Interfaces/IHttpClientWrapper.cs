namespace GameTracker.Plugins.Common.Interfaces
{
    public interface IHttpClientWrapper : IDisposable
    {
        Task<HttpResponseMessage> GetAsync(Uri requestUri);
        Task<HttpResponseMessage> GetAsync(string requestUri);
        Task<object> GetFromJsonAndTypeAsync(Uri requestUri, Type type);
        Task<string> GetStringAsync(string? requestUri);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}