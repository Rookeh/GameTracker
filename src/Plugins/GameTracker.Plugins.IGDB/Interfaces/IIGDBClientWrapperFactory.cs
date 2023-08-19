namespace GameTracker.Plugins.IGDB.Interfaces
{
    public interface IIGDBClientWrapperFactory
    {
        IIGDBClientWrapper BuildIGDBClientWrapper(string clientId, string clientSecret);
    }
}