using GameTracker.Plugins.Common.RateLimiting;
using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Interfaces.ApiClients;
using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.ApiClients
{
    public class RateLimitedSteamApiClient : RateLimitedHttpClient<Dictionary<string, SteamGameDetailsRoot>>, IRateLimitedSteamApiClient
    {
        private const int BackOffMinutes = 5;
        private const int MaxRequests = 18;

        public RateLimitedSteamApiClient()
            : base(TimeSpan.FromMinutes(BackOffMinutes), MaxRequests)
        {
        }

        public async Task<SteamGameDetails> GetSteamGameDetails(int appId)
        {
            var url = string.Format(Constants.ApiEndpoints.AppDetailsEndpoint, appId);
            var response = await GetFromJson(url, new Dictionary<string, SteamGameDetailsRoot>()
            {
                [appId.ToString()] = Constants.DefaultValues.DefaultSteamGameDetails
            });

            return response[appId.ToString()].Details;
        }
    }
}