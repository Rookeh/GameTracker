using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Interfaces.Data
{
    public interface IMetacriticScoreRepository
    {
        Task<MetacriticScore?> GetMetacriticScore(int appId);
        Task SetMetacriticScore(int appId, MetacriticScore metacriticScore);
    }
}