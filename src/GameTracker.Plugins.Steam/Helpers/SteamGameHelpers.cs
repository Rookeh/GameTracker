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
        internal static IEnumerable<MultiplayerMode> ParseMultiplayerModes(Category[]? categories)
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

        internal static IEnumerable<MultiplayerAvailability> ParseMultiplayerAvailability(Category[]? categories)
        {
            if (categories == null)
            {
                yield break;
            }

            if (categories.Any(c => c.Description.ToLower() == "multi-player"))
            {
                if (categories.Any(c => c.Description.ToLower().Contains("online")))
                {
                    yield return MultiplayerAvailability.Online;
                }

                if (categories.Any(c => c.Description.ToLower().Contains("local")))
                {
                    yield return MultiplayerAvailability.Local;
                }
            }
            else
            {
                yield return MultiplayerAvailability.None;
            }
        }

        internal static IEnumerable<GenreEnum> ParseGenres(Genre[]? genres)
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

        internal static Review[] ParseMetacriticReview(Game game, MetacriticScore? metacritic)
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
                    Game = game,
                    Content = metacritic.Url,
                    Score = metacritic.Score
                }
            };
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
    }
}