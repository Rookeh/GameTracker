using GameTracker.Plugins.Common.RateLimiting;
using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Singletons
{
    internal static class RateLimitedSteamApiClient
    {
        private const int BackOffMinutes = 5;
        private const int MaxRequests = 18;

        private static readonly RateLimitedHttpClient<Dictionary<string, SteamGameDetailsRoot>> RateLimitedHttpClient
            = new RateLimitedHttpClient<Dictionary<string, SteamGameDetailsRoot>>(TimeSpan.FromMinutes(BackOffMinutes), MaxRequests);

        internal static async Task<SteamGameDetails> GetSteamGameDetails(int appId)
        {
            var url = $"https://store.steampowered.com/api/appdetails?appids={appId}";
            var response = await RateLimitedHttpClient.GetFromJson(url, new Dictionary<string, SteamGameDetailsRoot>()
            {
                [appId.ToString()] = Constants.DefaultValues.DefaultSteamGameDetails
            });

            return response[appId.ToString()].Details;
        }
    }
}