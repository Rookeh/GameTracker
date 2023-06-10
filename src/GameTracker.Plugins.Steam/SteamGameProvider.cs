using GameTracker.Data;
using GameTracker.Interfaces;
using GameTracker.Models;
using GameTracker.Plugins.Steam.Interfaces;
using GameTracker.Plugins.Steam.Models;
using GameTracker.Plugins.Steam.Models.WebApi;
using System.Text.Json;

using Constants = GameTracker.Plugins.Steam.Helpers.Constants;
using LinkType = GameTracker.Models.Enums.LinkType;

namespace GameTracker.Plugins.Steam
{
    public class SteamGameProvider : IGameProvider
    {
        private readonly ISteamGameDetailsRepository _steamGameDetailsRepository;

        private readonly Platform _platform;                
        private readonly List<Game> _games;
        
        private bool _initialized;

        public SteamGameProvider(ISteamGameDetailsRepository steamGameDetailsRepository)
        {
            _games = new List<Game>();
            _platform = new Platform
            {
                Name = "Steam",
                Description = "Steam is a video game digital distribution service and storefront from Valve. It was launched as a software client in September 2003 as a way for Valve to provide automatic updates for their games, and expanded to distributing third-party game publishers' titles in late 2005.",
                Icon = "Steam",
                ExtendedInformation = @"This integration requires your Steam Web API key for authentication, and your profile's SteamID64 value to fetch game data.<br><br> " +
                                     @$"To obtain a Steam Web API key, visit <a href=""{Constants.Authentication.ApiKeyUrl}"">this</a> page. " +
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

            using HttpClient client = new();
            var userGameResponse = await client.GetAsync($"https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?key={apiKey}&steamid={steamId}");

            if (!userGameResponse.IsSuccessStatusCode)
            {
                string error = userGameResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized
                    ? "API key was rejected."
                    : $"Steam API returned status code {userGameResponse.StatusCode}.";

                throw new ApplicationException(error);
            }

            var userGameResponseJson = await userGameResponse.Content.ReadAsStringAsync();
            var userGames = JsonSerializer.Deserialize<SteamGameResponseRoot>(userGameResponseJson).Response;

            bool moreTitlesToQuery = true;
            int lastAppId = 0;
            _games.Clear();

            while (moreTitlesToQuery && _games.Count < userGames.GameCount)
            {
                var steamAppsJson = await client.GetStringAsync($"https://api.steampowered.com/IStoreService/GetAppList/v1/?key={apiKey}&last_appid={lastAppId}");
                var steamAppResponse = JsonSerializer.Deserialize<SteamAppResponseRoot>(steamAppsJson);
                var steamApps = steamAppResponse.Response.Apps;

                var userSteamApps = steamApps.Where(sa => userGames.Games.Select(ug => ug.AppId).Contains(sa.AppId));

                _games.AddRange(userSteamApps.Select(usa => new SteamGame(
                    _steamGameDetailsRepository,
                    usa,
                    userGames.Games.Single(g => g.AppId == usa.AppId).Playtime,
                    userGames.Games.Single(g => g.AppId == usa.AppId).LastPlayedTimestamp)));

                lastAppId = steamAppResponse.Response.LastAppId;
                moreTitlesToQuery = steamAppResponse.Response.HasMoreResults;
            }

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