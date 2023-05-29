using GameTracker.Models;
using GameTracker.Models.Constants;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Steam.Data;
using GameTracker.Plugins.Steam.Models.StoreApi;
using System.Text.Json;
using Genre = GameTracker.Plugins.Steam.Models.StoreApi.Genre;
using GenreEnum = GameTracker.Models.Enums.Genre;

namespace GameTracker.Plugins.Steam.Models
{
    public class SteamGame : Game
    {
        private SteamGameDetails _gameDetails;
        private readonly SteamGameDetailsRepository _gameDetailsRepository;

        public SteamGame(SteamGameDetailsRepository steamGameDetailsRepository, 
            int appId,
            int playTime,
            string title)
        {
            _gameDetailsRepository = steamGameDetailsRepository;
            PlatformId = appId;
            Playtime = TimeSpan.FromSeconds(playTime);
            Title = title;
        }

        public override string Description => GameDetails?.ShortDescription ?? string.Empty;
        public override GenreEnum[] Genres => ParseGenres(GameDetails?.Genres).ToArray();
        public override Uri Image => new Uri(GameDetails?.HeaderImage ?? string.Empty);
        public override string LaunchCommand => $"steam://run/{PlatformId}";
        public override MultiplayerAvailability[] MultiplayerAvailability => ParseMultiplayerAvailability(GameDetails?.Categories).ToArray();
        public override MultiplayerMode[] MultiplayerModes => ParseMultiplayerModes(GameDetails?.Categories).ToArray();
        public override Platform[] Platforms => ParsePlatforms(GameDetails?.Platforms).ToArray();
        public override Publisher Publisher => new Publisher { Name = GameDetails?.Publishers.FirstOrDefault() ?? "Unknown" };
        public override DateTime ReleaseDate => DateTime.Parse(GameDetails?.ReleaseDate.Date ?? DateTime.MaxValue.ToString());
        public override Review[] Reviews => ParseMetacriticReview(GameDetails?.Metacritic) ?? Array.Empty<Review>();
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
                var getGameDetailsFromSteamTask = GetGameDetailsFromSteam(PlatformId);
                getGameDetailsFromSteamTask.Wait();

                if (getGameDetailsFromSteamTask.IsCompleted && !getGameDetailsFromSteamTask.IsFaulted)
                {
                    var updateCacheTask = _gameDetailsRepository.SetGameDetails(getGameDetailsFromSteamTask.Result);
                    updateCacheTask.Wait();
                    return _gameDetails = getGameDetailsFromSteamTask.Result;
                }

                return null;
            }
        }

        private async Task<SteamGameDetails> GetGameDetailsFromSteam(int platformId)
        {
            using HttpClient client = new();
            var gameDetailResponse = await client.GetAsync($"https://store.steampowered.com/api/appdetails?appids={platformId}").ConfigureAwait(false);
            var gameDetailJson = await gameDetailResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var gameDetailObject = JsonSerializer.Deserialize<Dictionary<string, SteamGameDetailsRoot>>(gameDetailJson)[platformId.ToString()];
            return gameDetailObject.Details;
        }

        private IEnumerable<MultiplayerMode> ParseMultiplayerModes(Category[]? categories)
        {
            if (categories.Any(c => c.Description.ToLower() == "multi-player"))
            {
                if (categories.Any(c => c.Description.ToLower().Contains("co-op")))
                {
                    yield return MultiplayerMode.CoOp;
                }

                if (categories.Any(c => c.Description.ToLower().Contains("pve")))
                {
                    yield return MultiplayerMode.PvE;
                }

                if (categories.Any(c => c.Description.ToLower().Contains("pvp")))
                {
                    yield return MultiplayerMode.PvP;
                }
            }
            else
            {
                yield return MultiplayerMode.None;
            }
        }

        private IEnumerable<MultiplayerAvailability> ParseMultiplayerAvailability(Category[]? categories)
        {
            if (categories == null)
            {
                yield break;
            }

            if (categories.Any(c => c.Description.ToLower() == "multi-player"))
            {
                if (categories.Any(c => c.Description.ToLower().Contains("online")))
                {
                    yield return GameTracker.Models.Enums.MultiplayerAvailability.Online;
                }

                if (categories.Any(c => c.Description.ToLower().Contains("local")))
                {
                    yield return GameTracker.Models.Enums.MultiplayerAvailability.Local;
                }
            }
            else
            {
                yield return GameTracker.Models.Enums.MultiplayerAvailability.None;
            }
        }

        private IEnumerable<GenreEnum> ParseGenres(Genre[]? genres)
        {
            if (genres == null)
            {
                yield break;
            }

            foreach (var genre in genres)
            {
                switch (genre.Description.ToLower())
                {
                    case "action":
                        yield return GenreEnum.Action;
                        break;
                    case "adventure":
                        yield return GenreEnum.ActionAdventure;
                        break;
                    default:
                        yield return GenreEnum.Other;
                        break;
                }
            }
        }

        private Review[] ParseMetacriticReview(MetacriticScore? metacritic)
        {
            if (metacritic == null)
            {
                return Array.Empty<Review>();
            }

            return new[]
            {
                new Review
                {
                    Critic = WellKnownCritics.Metacritic,
                    Game = this,
                    Content = metacritic.Url,
                    Score = metacritic.Score
                }
            };
        }

        private IEnumerable<Platform> ParsePlatforms(Platforms? platforms)
        {
            if (platforms == null)
            {
                yield break;
            }

            if (platforms.Windows)
            {
                yield return WellKnownPlatforms.Windows;
            }

            if (platforms.Linux)
            {
                yield return WellKnownPlatforms.Linux;
            }

            if (platforms.Mac)
            {
                yield return WellKnownPlatforms.MacOS;
            }
        }

        #endregion

    }
}