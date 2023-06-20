using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Models.StoreApi;

using GenreEnum = GameTracker.Models.Enums.Genre;
using GameTracker.Plugins.Steam.Models.WebApi;
using GameTracker.Plugins.Steam.Interfaces.Data;
using GameTracker.Plugins.Steam.Interfaces.ApiClients;

namespace GameTracker.Plugins.Steam.Models
{
    public class SteamGame : Game
    {
        private readonly IRateLimitedSteamApiClient _rateLimitedApiClient;
        private readonly ISteamGameDetailsRepository _gameDetailsRepository;

        private SteamGameDetails? _extendedGameDetails;
        private SteamGameDto _gameDetails;
        private readonly DateTime _lastPlayed;
        private readonly TimeSpan _playTime;
        private readonly string _title;

        internal SteamGame(ref IRateLimitedSteamApiClient rateLimitedApiClient, ref ISteamGameDetailsRepository steamGameDetailsRepository, SteamGameDto steamGameDto)
        {            
            _rateLimitedApiClient = rateLimitedApiClient;
            _gameDetailsRepository = steamGameDetailsRepository;
            
            PlatformId = steamGameDto.AppId;
            _gameDetails = steamGameDto;
            _title = steamGameDto.Name;
            _lastPlayed = DateTime.UnixEpoch.AddSeconds(steamGameDto.LastPlayedTimestamp);
            _playTime = TimeSpan.FromMinutes(steamGameDto.Playtime);
        }

        public override async Task Preload()
        {
            _extendedGameDetails = await LazyLoadGameDetails();
        }

        public override ControlScheme[] ControlSchemes => SteamGameHelpers.ParseControlScheme(_extendedGameDetails?.Categories);

        public override string Description => _extendedGameDetails?.ShortDescription ?? string.Empty;

        public override GameplayMode[] GameplayModes => SteamGameHelpers.ParseMultiplayerModes(_extendedGameDetails?.Categories);

        public override GenreEnum[] Genres => SteamGameHelpers.ParseGenres(_extendedGameDetails?.Genres).Distinct().ToArray();

        public override Image Image => SteamGameHelpers.BuildImage(PlatformId, _gameDetails, _extendedGameDetails);

        public override DateTime? LastPlayed => _lastPlayed;

        public override LaunchCommand LaunchCommand => new LaunchCommand
        {
            Icon = "Steam",
            NewTab = false,
            Text = "Launch via Steam",
            Url = $"steam://run/{PlatformId}"
        };

        public override MultiplayerAvailability[] MultiplayerAvailability => SteamGameHelpers.ParseMultiplayerAvailability(_extendedGameDetails?.Categories);

        public override Platform[] Platforms => SteamGameHelpers.ParsePlatforms(_extendedGameDetails?.Platforms).ToArray();

        public override TimeSpan? Playtime => _playTime;

        public override Publisher? Publisher => SteamGameHelpers.ParsePublisher(_extendedGameDetails);

        public override DateTime? ReleaseDate
        {
            get
            {
                if (_extendedGameDetails?.ReleaseDate?.Date != null)
                {
                    if (DateTime.TryParse(_extendedGameDetails.ReleaseDate.Date, out var releaseDate))
                    {
                        return releaseDate;
                    }                    
                }

                return null;
            }
        }

        public override Review[] Reviews => SteamGameHelpers.ParseMetacriticReview(this, _extendedGameDetails?.Metacritic) ?? Array.Empty<Review>();

        public override string ProviderName => "Steam";

        public override Studio? Studio => _extendedGameDetails?.Developers?.Any() ?? false ? new Studio { Name = _extendedGameDetails.Developers.First() } : null;

        public override string[] Tags => _extendedGameDetails?.Categories?.Select(c => c.Description).ToArray() ?? Array.Empty<string>();    
        
        public override string Title => _title;

        #region Private methods

        /*
         * 
         * The below is necessary because Valve, in their infinite wisdom, do not provide any ability to batch requests for 
         * multiple titles into a single operation. Instead, we have to query their store API individually for each game.
         * 
         * To make this slightly less terrible, there is a SQLite cache which will store any metadata for a game once it has been
         * retrieved at least once. There is also a client-side rate limiter, which will prevent more than 20 requests every few 
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
            var steamGameDetails = await _rateLimitedApiClient.GetSteamGameDetails(PlatformId);

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