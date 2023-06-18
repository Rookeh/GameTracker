using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Interfaces.ApiClients
{
    public interface IRateLimitedSteamApiClient
    {
        Task<SteamGameDetails> GetSteamGameDetails(int appId);
    }
}