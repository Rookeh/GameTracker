using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Steam.Data;
using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Models.StoreApi;
using GameTracker.Plugins.Common.RateLimiting;

using GenreEnum = GameTracker.Models.Enums.Genre;

namespace GameTracker.Plugins.Steam.Models
{
    public class SteamGame : Game
    {
        private SteamGameDetails _gameDetails;
        private readonly SteamGameDetailsRepository _gameDetailsRepository;
        private readonly RateLimitedHttpClient<Dictionary<string, SteamGameDetailsRoot>> _rateLimitedHttpClient;

        private readonly TimeSpan _playTime;
        private readonly string _title;

        internal SteamGame(RateLimitedHttpClient<Dictionary<string, SteamGameDetailsRoot>> rateLimitedHttpClient,
            SteamGameDetailsRepository steamGameDetailsRepository, 
            int appId,
            int playTime,
            string title)
        {
            _gameDetailsRepository = steamGameDetailsRepository;
            _rateLimitedHttpClient = rateLimitedHttpClient;

            PlatformId = appId;
            _playTime = TimeSpan.FromMinutes(playTime);
            _title = title;
        }

        public override string Description => GameDetails?.ShortDescription ?? string.Empty;
        public override GenreEnum[] Genres => SteamGameHelpers.ParseGenres(GameDetails?.Genres).ToArray();
        public override string Image => GameDetails?.HeaderImage ?? "img\\placeholder.png";
        public override string LaunchCommand => $"steam://run/{PlatformId}";
        public override MultiplayerAvailability[] MultiplayerAvailability => SteamGameHelpers.ParseMultiplayerAvailability(GameDetails?.Categories).ToArray();
        public override MultiplayerMode[] MultiplayerModes => SteamGameHelpers.ParseMultiplayerModes(GameDetails?.Categories).ToArray();
        public override Platform[] Platforms => SteamGameHelpers.ParsePlatforms(GameDetails?.Platforms).ToArray();
        public override TimeSpan Playtime => _playTime;
        public override Publisher Publisher => new Publisher { Name = GameDetails?.Publishers.FirstOrDefault() ?? "Unknown" };
        public override DateTime ReleaseDate => DateTime.Parse(GameDetails?.ReleaseDate.Date ?? DateTime.MinValue.ToString());
        public override Review[] Reviews => SteamGameHelpers.ParseMetacriticReview(this, GameDetails?.Metacritic) ?? Array.Empty<Review>();
        public override Studio Studio => new Studio { Name = GameDetails?.Developers.FirstOrDefault() ?? "Unknown" };
        public override string[] Tags => GameDetails?.Categories.Select(c => c.Description).ToArray() ?? Array.Empty<string>();        
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

        private SteamGameDetails? GameDetails
        {
            get
            {
                // Have we cached the game details in memory?
                return _gameDetails ?? LazyLoadGameDetails();
            }
        }

        private SteamGameDetails? LazyLoadGameDetails()
        {
            // Have we cached the game details in Sqlite?
            var getGameDetailsFromCacheTask = _gameDetailsRepository.GetGameDetails(PlatformId);
            getGameDetailsFromCacheTask.Wait();

            if (getGameDetailsFromCacheTask.IsCompleted
                && !getGameDetailsFromCacheTask.IsFaulted
                && getGameDetailsFromCacheTask.Result != null)
            {
                return _gameDetails = getGameDetailsFromCacheTask.Result;
            }

            // Otherwise, we have to fetch the game details from Steam (this is expensive, and we may be rate limited).
            var getGameDetailsFromSteamTask = _rateLimitedHttpClient.GetFromJson(
                $"https://store.steampowered.com/api/appdetails?appids={PlatformId}",
                new Dictionary<string, SteamGameDetailsRoot>()
                {
                    [PlatformId.ToString()] = Constants.GetDefaultSteamGameDetails()
                });

            getGameDetailsFromSteamTask.Wait();

            if (getGameDetailsFromSteamTask.IsCompleted && !getGameDetailsFromSteamTask.IsFaulted)
            {
                var steamGameDetails = getGameDetailsFromSteamTask.Result[PlatformId.ToString()].Details;

                if (!steamGameDetails.IsDefaultValue)
                {
                    var updateCacheTask = _gameDetailsRepository.SetGameDetails(steamGameDetails);
                    updateCacheTask.Wait();
                    return _gameDetails = steamGameDetails;
                }

                return steamGameDetails;
            }

            return null;
        }

        #endregion

    }
}