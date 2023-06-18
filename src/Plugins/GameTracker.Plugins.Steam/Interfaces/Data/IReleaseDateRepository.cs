using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Interfaces.Data
{
    public interface IReleaseDateRepository
    {
        Task<ReleaseDate?> GetReleaseDate(int appId);
        Task SetReleaseDate(int appId, ReleaseDate releaseDate);
    }
}