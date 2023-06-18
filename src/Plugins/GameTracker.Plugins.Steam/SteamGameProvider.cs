using GameTracker.Data;
using GameTracker.Interfaces;
using GameTracker.Models;
using GameTracker.Plugins.Common.Helpers;
using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.Steam.Interfaces.ApiClients;
using GameTracker.Plugins.Steam.Interfaces.Data;
using GameTracker.Plugins.Steam.Models;
using GameTracker.Plugins.Steam.Models.WebApi;
using System.Text.Json;

using Constants = GameTracker.Plugins.Steam.Helpers.Constants;
using LinkType = GameTracker.Models.Enums.LinkType;

namespace GameTracker.Plugins.Steam
{
    public class SteamGameProvider : IGameProvider
    {
        private readonly IHttpClientWrapperFactory _httpClientFactory;

        private IRateLimitedSteamApiClient _rateLimitedSteamApiClient;
        private ISteamGameDetailsRepository _steamGameDetailsRepository;

        private readonly Platform _platform;                
        private readonly List<Game> _games;        
        private bool _initialized;

        public SteamGameProvider(IHttpClientWrapperFactory httpClientFactory,
            IRateLimitedSteamApiClient rateLimitedSteamApiClient,
            ISteamGameDetailsRepository steamGameDetailsRepository)
        {
            _games = new List<Game>();
            _httpClientFactory = httpClientFactory;
            _rateLimitedSteamApiClient = rateLimitedSteamApiClient;
            _platform = new Platform
            {
                Name = "Steam",
                Description = "Steam is a digital storefront and game delivery service for the PC developed and operated by Valve. It was launched in 2003 as an update mechanism " +
                              "for early Valve titles such as Counter-Strike 1.6, and soon afterwards it was used as the launch platform for Valve's critically-acclaimed Half-Life 2. " +
                              "Since then, its storefront expanded to include non-Valve titles, and within a decade it became a highly dominant presence on the Windows PC platform, with over " +
                              "50,000 titles available for purchase as of December 2022. In 2010 support for Mac OS X was added, and in 2013 a Linux client was introduced. Since then, " +
                              "Valve has placed a strong emphasis on cross-OS compatibility - going so far as to build their own Linux compatibility layer, Proton. This culminated in " +
                              "2022 with the release of the Steam Deck, a handheld gaming computer running SteamOS - a custom Linux distribution based on Arch.",
                Icon = "Steam",
                ExtendedInformation = @"This integration requires your Steam Web API key for authentication, and your profile's SteamID64 value to fetch game data.<br><br> " +
                                     @$"To obtain a Steam Web API key, visit <a href=""{Constants.Authentication.ApiKeyUrl}"">this</a> page.<br><br> " +
                                     @$"To find your SteamID64 value, use a tool like <a href=""{Constants.Authentication.SteamIdUrl}"">steamid.io</a>.",                                
                Links = new[]
                {
                    new SocialLink
                    {
                        LinkPlatform = LinkType.Web,
                        LinkTarget = "https://store.steampowered.com/"
                    },
                    new SocialLink
                    {
                        LinkPlatform = LinkType.Twitter,
                        LinkTarget = "@Steam"
                    },
                    new SocialLink
                    {
                        LinkPlatform = LinkType.YouTube,
                        LinkTarget = "@Steam"
                    }
                }
            };

            _steamGameDetailsRepository = steamGameDetailsRepository;
        }

        public IEnumerable<Game> Games => _games;

        public bool Initialized => _initialized;

        public Platform Platform => _platform;

        public Guid ProviderId => new Guid("C53E9BCB-B519-4888-A16C-849BE2B7B77B");

        public async Task Load(ParameterCache parameterCache)
        {
            await Refresh(parameterCache.UserId, parameterCache.Parameters);
        }

        public async Task<ParameterCache> Refresh(string userId, params object[] providerSpecificParameters)
        {
            /*
             * https://steamapi.xpaw.me/
             * https://wiki.teamfortress.com/wiki/User:RJackson/StorefrontAPI
             * 
             * This is pretty terrible. We have to make one operation to get the games for the account (fair enough),
             * but then to get extended metadata, we have to make a query for every single title the user owns individually.
             * 
             * Hence, all of the logic to fetch title metadata is delegated to SteamGame.cs, where it will be lazily loaded
             * (and then cached) on-demand. This is not ideal for when we need to sort the total set of games based on these
             * extended properties - any games that do not have these available will have some placeholders instead - but there
             * is not really any way around this unless Valve overhaul their API in the future.
             * 
             */

            if (providerSpecificParameters.Length != 2)
            {
                throw new ArgumentException("Steam Web API key and SteamID64 must be provided.");
            }

            var apiKey = providerSpecificParameters[0].ToString();
            var steamId = providerSpecificParameters[1].ToString();
            var ownedGameParams = new Dictionary<string, string>()
            {
                ["key"] = apiKey,
                ["steamid"] = steamId,
                ["include_appinfo"] = "true",
                ["include_played_free_games"] = "true",
                ["include_free_sub"] = "true",
                ["include_extended_appinfo"] = "true",
                ["format"] = "json"
            };

            var ownedGamesRequestUri = UriHelper.BuildQueryString(Constants.ApiEndpoints.OwnedGamesEndpoint, ownedGameParams);
            using var client = _httpClientFactory.BuildHttpClient();
            var userGameResponse = await client.GetAsync(ownedGamesRequestUri);

            if (!userGameResponse.IsSuccessStatusCode)
            {
                string error = userGameResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized
                    ? "API key was rejected."
                    : $"Steam API returned status code {userGameResponse.StatusCode}.";

                throw new ApplicationException(error);
            }

            var userGameResponseJson = await userGameResponse.Content.ReadAsStringAsync();
            var userGames = JsonSerializer.Deserialize<SteamGameResponseRoot>(userGameResponseJson).Response;

            _games.Clear();

            _games.AddRange(userGames.Games.Select(ug => new SteamGame(ref _rateLimitedSteamApiClient, ref _steamGameDetailsRepository, ug)));

            _initialized = true;

            return new ParameterCache
            {
                Parameters = providerSpecificParameters,
                ProviderId = ProviderId,
                UserId = userId
            };
        }

        public Dictionary<string, Type> RequiredParameters => new()
        {
            { "Steam Web API Key", typeof(string) },
            { "SteamID64", typeof(string) }
        };
    }
}