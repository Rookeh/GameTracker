using GameTracker.Interfaces;
using GameTracker.Models;
using GameTracker.Plugins.Steam.Data;
using GameTracker.Plugins.Steam.Models;
using GameTracker.Plugins.Steam.Models.WebApi;
using System.Text.Json;
using LinkType = GameTracker.Models.Enums.LinkType;

namespace GameTracker.Plugins.Steam
{
    public class SteamGameProvider : IGameProvider
    {
        private readonly Platform _platform;
        private readonly SteamGameDetailsRepository _steamGameDetailsRepository;
        
        private List<Game> _games;

        public SteamGameProvider()
        {
            _games = new List<Game>();
            _platform = new Platform
            {
                Name = "Steam",
                Description = "Steam is a video game digital distribution service and storefront from Valve. It was launched as a software client in September 2003 as a way for Valve to provide automatic updates for their games, and expanded to distributing third-party game publishers' titles in late 2005.",
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
            _steamGameDetailsRepository = new SteamGameDetailsRepository();
        }

        public IEnumerable<Game> Games => _games;

        public Platform Platform => _platform;

        public Guid ProviderId => new Guid("C53E9BCB-B519-4888-A16C-849BE2B7B77B");

        public async Task Refresh(params object[] providerSpecificParameters)
        {
            if (providerSpecificParameters.Length != 2)
            {
                throw new ArgumentException("Steam WebAPI key and SteamID must be provided.");
            }

            var apiKey = providerSpecificParameters[0].ToString();
            var steamId = providerSpecificParameters[1].ToString();

            using HttpClient client = new();
            var userGameJson = await client.GetStringAsync($"https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?key={apiKey}&steamid={steamId}");
            var userGames = JsonSerializer.Deserialize<SteamGameResponseRoot>(userGameJson).Response;

            bool moreTitlesToQuery = true;
            int lastAppId = 0;

            while (moreTitlesToQuery && _games.Count < userGames.GameCount)
            {
                var steamAppsJson = await client.GetStringAsync($"https://api.steampowered.com/IStoreService/GetAppList/v1/?key={apiKey}&last_appid={lastAppId}");
                var steamAppResponse = JsonSerializer.Deserialize<SteamAppResponseRoot>(steamAppsJson);
                var steamApps = steamAppResponse.Response.Apps;

                var userSteamApps = steamApps.Where(sa => userGames.Games.Select(ug => ug.AppId).Contains(sa.AppId));
                
                _games.AddRange(userSteamApps.Select(usa => new SteamGame(
                    _steamGameDetailsRepository,
                    usa.AppId, 
                    userGames.Games.Single(g => g.AppId == usa.AppId).Playtime,
                    usa.Name)));

                lastAppId = steamAppResponse.Response.LastAppId;
                moreTitlesToQuery = steamAppResponse.Response.HasMoreResults;
            }
        }

        public Dictionary<string, Type> RequiredParameters => new()
        {
            { "Steam WebAPI Key", typeof(string) },
            { "SteamID", typeof(string) }
        };
    }
}