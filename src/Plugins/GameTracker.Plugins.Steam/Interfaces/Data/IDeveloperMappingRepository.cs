using GameTracker.Plugins.Steam.Models.Database;

namespace GameTracker.Plugins.Steam.Interfaces.Data
{
    public interface IDeveloperMappingRepository
    {
        Task<IEnumerable<DeveloperMapping>> GetDeveloperMappings(int appId);
        Task InsertDeveloperMapping(DeveloperMapping mapping);
    }
}