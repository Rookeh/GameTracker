using GameTracker.Interfaces;
using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.EpicGames.Models;

namespace GameTracker.Plugins.EpicGames
{
    public class EpicGamesGameProvider : IGameProvider
    {
        private readonly List<Game> _games;
        private bool _initialized;

        public EpicGamesGameProvider()
        {
            _games = new List<Game>();
        }

        public Guid ProviderId => new Guid("16C98CD0-3BDC-4A0A-93E1-E992D8C00926");

        public Platform Platform => new Platform
        {
            Name = "Epic Games",
            Description = "The Epic Games Store is a digital video game storefront for Microsoft Windows and macOS, operated by Epic Games.",
            Links = new[]
            {
                new SocialLink { LinkPlatform = LinkType.Web, LinkTarget = "https://store.epicgames.com" },
                new SocialLink { LinkPlatform = LinkType.Twitter, LinkTarget = "@EpicGames "}
            }
        };

        public IEnumerable<Game> Games => _games;

        public Dictionary<string, Type> RequiredParameters => new()
        {
            { "Semicolon-delimited list of titles", typeof(string) }
        };

        public bool Initialized => _initialized;

        public async Task Load(ParameterCache parameterCache)
        {
            await Refresh(parameterCache.UserId, parameterCache.Parameters);
        }

        public async Task<ParameterCache> Refresh(string userId, params object[] providerSpecificParameters)
        {
            if (providerSpecificParameters[0] == null || !(providerSpecificParameters[0] is string))
            {
                throw new ArgumentException("Semicolon-delimited list of titles must be provided.");
            }

            var gameTitles = providerSpecificParameters[0].ToString().Split(';');

            _games.Clear();

            var gameResponses = await Task.WhenAll(gameTitles.Select(gt => EpicGamesStoreNET.Query.SearchAsync(gt)));
            var epicGames = gameResponses.Where(gr => gr.Data?.Catalog?.SearchStore?.Elements != null && gr.Data.Catalog.SearchStore.Elements.Any())
                .Select(gr => new EpicGame(gr.Data.Catalog.SearchStore.Elements.First()))
                .ToList();

            _games.AddRange(epicGames);

            _initialized = true;

            return new ParameterCache
            {
                Parameters = providerSpecificParameters,
                ProviderId = ProviderId,
                UserId = userId
            };
        }
    }
}