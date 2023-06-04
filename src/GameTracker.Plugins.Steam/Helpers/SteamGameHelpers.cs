using GameTracker.Models.Constants;
using GameTracker.Models.Enums;
using GameTracker.Models;
using GameTracker.Plugins.Steam.Models.StoreApi;

using Genre = GameTracker.Plugins.Steam.Models.StoreApi.Genre;
using GenreEnum = GameTracker.Models.Enums.Genre;

namespace GameTracker.Plugins.Steam.Helpers
{
    internal static class SteamGameHelpers
    {
        internal static IEnumerable<GameplayMode> ParseMultiplayerModes(Category[]? categories)
        {
            var multiplayerModes = new List<GameplayMode>();

            if (categories == null || !categories.Any())
            {
                return multiplayerModes;
            }

            foreach (var category in categories)
            {
                if (Constants.SteamCategoryMappings.SteamMultiplayerModeMappings.ContainsKey(category.Description))
                {
                    multiplayerModes.Add(Constants.SteamCategoryMappings.SteamMultiplayerModeMappings[category.Description]);
                }
            }

            return multiplayerModes.Distinct();
        }

        internal static IEnumerable<MultiplayerAvailability> ParseMultiplayerAvailability(Category[]? categories)
        {
            var multiplayerAvailability = new List<MultiplayerAvailability>();

            if (categories == null || !categories.Any())
            {
                return multiplayerAvailability;
            }

            foreach (var category in categories)
            {
                if (Constants.SteamCategoryMappings.SteamMultiplayerAvailMappings.ContainsKey(category.Description))
                {
                    multiplayerAvailability.Add(Constants.SteamCategoryMappings.SteamMultiplayerAvailMappings[category.Description]);
                }
            }

            return multiplayerAvailability.Distinct();
        }

        internal static IEnumerable<GenreEnum> ParseGenres(Genre[]? genres)
        {
            if (genres == null || !genres.Any())
            {
                yield break;
            }

            foreach (var genre in genres)
            {
                yield return Constants.SteamCategoryMappings.SteamGenreMappings.ContainsKey(genre.Description)
                    ? Constants.SteamCategoryMappings.SteamGenreMappings[genre.Description]
                    : GenreEnum.Other;
            }
        }

        internal static Review[] ParseMetacriticReview(Game game, MetacriticScore? metacritic)
        {
            return metacritic == null
                ? Array.Empty<Review>()
                : (new[]
                {
                    new Review
                    {
                        Critic = WellKnownCritics.Metacritic,
                        Game = game,
                        Content = metacritic.Url,
                        Score = metacritic.Score
                    }
                });
        }

        internal static IEnumerable<Platform> ParsePlatforms(Platforms? platforms)
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

        internal static Publisher? ParsePublisher(SteamGameDetails gameDetails)
        {
            if (gameDetails.Publishers == null || !gameDetails.Publishers.Any())
            {
                return null;
            }

            return new Publisher { Name = gameDetails.Publishers.First() };
        }
    }
}