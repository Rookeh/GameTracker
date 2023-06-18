using GameTracker.Plugins.Steam.Models.Database;

namespace GameTracker.Plugins.Steam.Interfaces.Data
{
    public interface IPublisherMappingRepository
    {
        Task<IEnumerable<PublisherMapping>> GetPublisherMappings(int appId);
        Task InsertPublisherMapping(PublisherMapping mapping);
    }
}