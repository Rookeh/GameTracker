using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Steam.Data;
using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Models.StoreApi;
using GameTracker.Plugins.Common.RateLimiting;

using GenreEnum = GameTracker.Models.Enums.Genre;
using GameTracker.Plugins.Steam.Models.WebApi;

namespace GameTracker.Plugins.Steam.Models
{
    public class SteamGame : Game
    {
        private SteamGameDetails? _gameDetails;
        private readonly SteamGameDetailsRepository _gameDetailsRepository;
        private readonly RateLimitedHttpClient<Dictionary<string, SteamGameDetailsRoot>> _rateLimitedHttpClient;

        private readonly DateTime _lastPlayed;
        private readonly TimeSpan _playTime;
        private readonly string _title;

        internal SteamGame(RateLimitedHttpClient<Dictionary<string, SteamGameDetailsRoot>> rateLimitedHttpClient,
            SteamGameDetailsRepository steamGameDetailsRepository,
            SteamApp app,
            int playTime,
            long lastPlayed)
        {
            PlatformId = app.AppId;
            _gameDetailsRepository = steamGameDetailsRepository;
            _rateLimitedHttpClient = rateLimitedHttpClient;
            _title = app.Name;

            _lastPlayed = DateTime.UnixEpoch.AddSeconds(lastPlayed);
            _playTime = TimeSpan.FromMinutes(playTime);
        }

        public override async Task Preload()
        {
            _gameDetails = await LazyLoadGameDetails();
        }

        public override string Description => _gameDetails?.ShortDescription ?? string.Empty;
        public override GameplayMode[] GameplayModes => SteamGameHelpers.ParseMultiplayerModes(_gameDetails?.Categories).ToArray();
        public override GenreEnum[] Genres => SteamGameHelpers.ParseGenres(_gameDetails?.Genres).ToArray();
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
            Uri = $"steam://run/{PlatformId}"
        };
        public override MultiplayerAvailability[] MultiplayerAvailability => SteamGameHelpers.ParseMultiplayerAvailability(_gameDetails?.Categories).ToArray();
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
        public override Studio? Studio => _gameDetails.Developers.Any() ? new Studio { Name = _gameDetails.Developers.First() } : null;
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
            var steamApiResults = await _rateLimitedHttpClient.GetFromJson(
                $"https://store.steampowered.com/api/appdetails?appids={PlatformId}",
                new Dictionary<string, SteamGameDetailsRoot>()
                {
                    [PlatformId.ToString()] = Constants.DefaultValues.DefaultSteamGameDetails
                });

            if (steamApiResults != null && steamApiResults.ContainsKey(PlatformId.ToString()))
            {
                var steamGameDetails = steamApiResults[PlatformId.ToString()].Details;

                if (steamGameDetails != null)
                {
                    if (!steamGameDetails.IsDefaultValue)
                    {
                        await _gameDetailsRepository.SetGameDetails(steamGameDetails);
                    }

                    return steamGameDetails;
                }
            }

            return Constants.DefaultValues.DefaultSteamGameDetails.Details;
        }

        #endregion
    }
}