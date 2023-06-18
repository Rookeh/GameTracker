using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Interfaces.Data
{
    public interface ICategoryRepository
    {
        Task<Category[]> GetCategories(int appId);
        Task SetCategories(int appId, Category[] categories);
    }
}