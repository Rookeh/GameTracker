using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Interfaces
{
    public interface ISteamGameDetailsRepository
    {
        Task<SteamGameDetails?> GetGameDetails(int appId);
        Task SetGameDetails(SteamGameDetails steamGameDetails);
    }
}