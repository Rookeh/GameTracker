using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Interfaces.Data
{
    public interface IGenreRepository
    {
        Task<Genre[]> GetGenres(int appId);
        Task SetGenres(int appId, Genre[] genres);
    }
}