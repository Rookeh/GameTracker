namespace GameTracker.Plugins.Common.Interfaces
{
    public interface IHttpClientWrapperFactory
    {
        IHttpClientWrapper BuildHttpClient();
    }
}