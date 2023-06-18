using GameTracker.Plugins.Steam.Models.Database;

namespace GameTracker.Plugins.Steam.Interfaces.Data
{
    public interface ICategoryMappingRepository
    {
        Task<IEnumerable<CategoryMapping>> GetCategoryMappings(int appId);
        Task InsertCategoryMapping(CategoryMapping mapping);
    }
}