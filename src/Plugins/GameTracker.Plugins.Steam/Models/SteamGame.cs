using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Models.StoreApi;

using GenreEnum = GameTracker.Models.Enums.Genre;
using GameTracker.Plugins.Steam.Models.WebApi;
using GameTracker.Plugins.Steam.Singletons;
using GameTracker.Plugins.Steam.Interfaces;

namespace GameTracker.Plugins.Steam.Models
{
    public class SteamGame : Game
    {
        private readonly ISteamGameDetailsRepository _gameDetailsRepository;

        private SteamGameDetails? _gameDetails;
        private readonly DateTime _lastPlayed;
        private readonly TimeSpan _playTime;
        private readonly string _title;

        internal SteamGame(ISteamGameDetailsRepository steamGameDetailsRepository, SteamGameDto steamGameDto)
        {
            PlatformId = steamGameDto.AppId;
            _gameDetailsRepository = steamGameDetailsRepository;
            _title = steamGameDto.Name;
            _lastPlayed = DateTime.UnixEpoch.AddSeconds(steamGameDto.LastPlayedTimestamp);
            _playTime = TimeSpan.FromMinutes(steamGameDto.Playtime);
        }

        public override async Task Preload()
        {
            _gameDetails = await LazyLoadGameDetails();
        }

        public override ControlScheme[] ControlSchemes => SteamGameHelpers.ParseControlScheme(_gameDetails?.Categories);

        public override string Description => _gameDetails?.ShortDescription ?? string.Empty;

        public override GameplayMode[] GameplayModes => SteamGameHelpers.ParseMultiplayerModes(_gameDetails?.Categories);

        public override GenreEnum[] Genres => SteamGameHelpers.ParseGenres(_gameDetails?.Genres).Distinct().ToArray();

        public override Image Image => new Image()
        {
            Url = _gameDetails?.HeaderImage ?? "img\\placeholder.png",
            Width = 460,
            Height = 215
        };

        public override DateTime? LastPlayed => _lastPlayed;

        public override LaunchCommand LaunchCommand => new LaunchCommand
        {
            Icon = "Steam",
            NewTab = false,
            Text = "Launch via Steam",
            Url = $"steam://run/{PlatformId}"
        };

        public override MultiplayerAvailability[] MultiplayerAvailability => SteamGameHelpers.ParseMultiplayerAvailability(_gameDetails?.Categories);

        public override Platform[] Platforms => SteamGameHelpers.ParsePlatforms(_gameDetails?.Platforms).ToArray();

        public override TimeSpan? Playtime => _playTime;

        public override Publisher? Publisher => SteamGameHelpers.ParsePublisher(_gameDetails);

        public override DateTime? ReleaseDate
        {
            get
            {
                if (_gameDetails?.ReleaseDate?.Date != null)
                {
                    if (DateTime.TryParse(_gameDetails.ReleaseDate.Date, out var releaseDate))
                    {
                        return releaseDate;
                    }                    
                }

                return null;
            }
        }

        public override Review[] Reviews => SteamGameHelpers.ParseMetacriticReview(this, _gameDetails?.Metacritic) ?? Array.Empty<Review>();

        public override string StorefrontName => "Steam";

        public override Studio? Studio => _gameDetails?.Developers?.Any() ?? false ? new Studio { Name = _gameDetails.Developers.First() } : null;

        public override string[] Tags => _gameDetails?.Categories.Select(c => c.Description).ToArray() ?? Array.Empty<string>();    
        
        public override string Title => _title;

        #region Private methods

        /*
         * 
         * The below is necessary because Valve, in their infinite wisdom, do not provide any ability to batch requests for 
         * multiple titles into a single operation. Instead, we have to query their store API individually for each game.
         * 
         * To make this slightly less terrible, there is a SQLite cache which will store any metadata for a game once it has been
         * retrieved at least once. There is also a client-side rate limiter, which will prevent more than 15 requests every few 
         * minutes (beyond this, it seems that we trigger a temporary block on Steam's CDN). Any games that fail to obtain metadata
         * due to rate-limiting will have some basic information populated, but extended metadata + images will be replaced by
         * placeholders, until a successful request can be made.
         * 
         * Users with large Steam libraries are more likely to be adversely affected by this limitation.
         * 
         */

        private async Task<SteamGameDetails?> LazyLoadGameDetails()
        {
            // Have we cached the game details in Sqlite?
            var cachedGameDetails = await _gameDetailsRepository.GetGameDetails(PlatformId);
            if (cachedGameDetails != null)
            {
                return cachedGameDetails;
            }

            // Otherwise, we have to fetch the game details from Steam (this is expensive, and we may be rate limited).
            var steamGameDetails = await RateLimitedSteamApiClient.GetSteamGameDetails(PlatformId);

            if (steamGameDetails != null)
            {
                if (!steamGameDetails.IsDefaultValue)
                {
                    await _gameDetailsRepository.SetGameDetails(steamGameDetails);
                }

                return steamGameDetails;
            }

            return Constants.DefaultValues.DefaultSteamGameDetails.Details;
        }

        #endregion
    }
}