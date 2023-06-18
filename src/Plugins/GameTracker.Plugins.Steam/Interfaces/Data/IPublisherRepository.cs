namespace GameTracker.Plugins.Steam.Interfaces.Data
{
    public interface IPublisherRepository
    {
        Task<string[]> GetPublishers(int appId);
        Task SetPublishers(int appId, string[] publishers);
    }
}