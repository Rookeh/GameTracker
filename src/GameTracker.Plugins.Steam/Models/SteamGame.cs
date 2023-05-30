using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Steam.Data;
using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Models.StoreApi;
using GameTracker.Plugins.Steam.RateLimiting;

using GenreEnum = GameTracker.Models.Enums.Genre;

namespace GameTracker.Plugins.Steam.Models
{
    public class SteamGame : Game
    {
        private SteamGameDetails _gameDetails;
        private readonly SteamGameDetailsRepository _gameDetailsRepository;
        private readonly RateLimitedHttpClient<Dictionary<string, SteamGameDetailsRoot>> _rateLimitedHttpClient;

        internal SteamGame(RateLimitedHttpClient<Dictionary<string, SteamGameDetailsRoot>> rateLimitedHttpClient,
            SteamGameDetailsRepository steamGameDetailsRepository, 
            int appId,
            int playTime,
            string title)
        {
            _gameDetailsRepository = steamGameDetailsRepository;
            _rateLimitedHttpClient = rateLimitedHttpClient;

            PlatformId = appId;
            Playtime = TimeSpan.FromMinutes(playTime);
            Title = title;
        }

        public override string Description => GameDetails?.ShortDescription ?? string.Empty;
        public override GenreEnum[] Genres => SteamGameHelpers.ParseGenres(GameDetails?.Genres).ToArray();
        public override string Image => GameDetails?.HeaderImage ?? "img\\placeholder.png";
        public override string LaunchCommand => $"steam://run/{PlatformId}";
        public override MultiplayerAvailability[] MultiplayerAvailability => SteamGameHelpers.ParseMultiplayerAvailability(GameDetails?.Categories).ToArray();
        public override MultiplayerMode[] MultiplayerModes => SteamGameHelpers.ParseMultiplayerModes(GameDetails?.Categories).ToArray();
        public override Platform[] Platforms => SteamGameHelpers.ParsePlatforms(GameDetails?.Platforms).ToArray();
        public override Publisher Publisher => new Publisher { Name = GameDetails?.Publishers.FirstOrDefault() ?? "Unknown" };
        public override DateTime ReleaseDate => DateTime.Parse(GameDetails?.ReleaseDate.Date ?? DateTime.MaxValue.ToString());
        public override Review[] Reviews => SteamGameHelpers.ParseMetacriticReview(this, GameDetails?.Metacritic) ?? Array.Empty<Review>();
        public override Studio Studio => new Studio { Name = GameDetails?.Developers.FirstOrDefault() ?? "Unknown" };
        public override string[] Tags => GameDetails?.Categories.Select(c => c.Description).ToArray() ?? Array.Empty<string>();

        #region Private methods

        private SteamGameDetails? GameDetails
        {
            get
            {
                // Have we cached the game details in memory?
                if (_gameDetails != null)
                {
                    return _gameDetails;
                }

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
                var getGameDetailsFromSteamTask = _rateLimitedHttpClient.Get(
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
        }

        #endregion

    }
}