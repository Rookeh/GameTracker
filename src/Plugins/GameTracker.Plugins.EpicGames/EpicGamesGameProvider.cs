using GameTracker.Interfaces;
using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.EpicGames.Interfaces;
using GameTracker.Plugins.EpicGames.Models;

namespace GameTracker.Plugins.EpicGames
{
    public class EpicGamesGameProvider : IGameProvider
    {
        private readonly IEpicGamesStore _epicGamesStore;
        private readonly List<Game> _games;        
        private bool _initialized;

        public EpicGamesGameProvider(IEpicGamesStore epicGamesStore)
        {            
            _epicGamesStore = epicGamesStore;
            _games = new List<Game>();
        }

        public Guid ProviderId => new Guid("16C98CD0-3BDC-4A0A-93E1-E992D8C00926");

        public DataPlatform Platform => new DataPlatform
        {
            Name = "Epic Games Store",
            Description = "The Epic Games Store is a digital storefront for video games, developed by Epic Games and offered on Windows and macOS. " + 
                          "It was launched in 2018 and competes with other distribution platforms on the PC - including Steam, GOG and the Xbox Network.",
            ExtendedInformation = "This integration cannot access your Epic account; therefore, you must specify which titles you own. " + 
                                  "Details for owned titles will be retrieved from the Epic Games store.<br><br> " +
                                  "Note that due to API limitations, not all titles support deep linking to the Epic Games Launcher. " + 
                                  "If this is not possible for a given title, you will be directed to the title's store page instead.",
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
            if (!providerSpecificParameters.Any() || providerSpecificParameters[0] == null || !(providerSpecificParameters[0] is string))
            {
                throw new ArgumentException("Semicolon-delimited list of titles must be provided.");
            }

            var gameTitles = providerSpecificParameters[0].ToString().Split(';');

            _games.Clear();

            var gameResponses = await Task.WhenAll(gameTitles.Select(gt => _epicGamesStore.SearchAsync(gt)));
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