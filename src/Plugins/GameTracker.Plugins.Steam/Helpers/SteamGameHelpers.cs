using GameTracker.Models.Constants;
using GameTracker.Models.Enums;
using GameTracker.Models;
using GameTracker.Plugins.Steam.Models.StoreApi;

using Genre = GameTracker.Plugins.Steam.Models.StoreApi.Genre;
using GenreEnum = GameTracker.Models.Enums.Genre;
using GameTracker.Plugins.Steam.Models.WebApi;

namespace GameTracker.Plugins.Steam.Helpers
{
    internal static class SteamGameHelpers
    {
        internal static Image BuildImage(int appId, SteamGameDto details, SteamGameDetails? extendedDetails)
        {
            if (extendedDetails?.HeaderImage != null)
            {
                return new Image()
                {
                    Url = extendedDetails.HeaderImage,
                    Width = 460,
                    Height = 215
                };
            }
            else if (!string.IsNullOrEmpty(details.IconId))
            {
                return new Image()
                {
                    Url = string.Format(Constants.ApiEndpoints.AlternativeImageBaseUrl, appId, details.IconId),
                    Width = 184,
                    Height = 69
                };
            }
            else
            {
                return new Image()
                {
                    Url = "img\\placeholder.png",
                    Width = 460,
                    Height = 215
                };
            }
        }

        internal static ControlScheme[] ParseControlScheme(Category[]? categories)
        {
            var controlSchemes = new List<ControlScheme>()
            {
                ControlScheme.KeyboardMouse
            };

            if (categories == null || !categories.Any())
            {
                return controlSchemes.ToArray();
            }

            foreach (var category in categories)
            {
                if (Constants.SteamCategoryMappings.SteamControlSchemeMappings.ContainsKey(category.Description))
                {
                    controlSchemes.Add(Constants.SteamCategoryMappings.SteamControlSchemeMappings[category.Description]);
                }
            }

            if (controlSchemes.Contains(ControlScheme.VROnly))
            {                
                controlSchemes.Remove(ControlScheme.KeyboardMouse);
                controlSchemes.Remove(ControlScheme.VRSupported);
            }

            return controlSchemes.Distinct().ToArray();
        }

        internal static GameplayMode[] ParseMultiplayerModes(Category[]? categories)
        {
            var multiplayerModes = new List<GameplayMode>();

            if (categories == null || !categories.Any())
            {
                return multiplayerModes.ToArray();
            }

            foreach (var category in categories)
            {
                if (Constants.SteamCategoryMappings.SteamMultiplayerModeMappings.ContainsKey(category.Description))
                {
                    multiplayerModes.Add(Constants.SteamCategoryMappings.SteamMultiplayerModeMappings[category.Description]);
                }
            }

            return multiplayerModes.Distinct().ToArray();
        }

        internal static MultiplayerAvailability[] ParseMultiplayerAvailability(Category[]? categories)
        {
            var multiplayerAvailability = new List<MultiplayerAvailability>();

            if (categories == null || !categories.Any())
            {
                return multiplayerAvailability.ToArray();
            }

            foreach (var category in categories)
            {
                if (Constants.SteamCategoryMappings.SteamMultiplayerAvailMappings.ContainsKey(category.Description))
                {
                    multiplayerAvailability.Add(Constants.SteamCategoryMappings.SteamMultiplayerAvailMappings[category.Description]);
                }
            }

            return multiplayerAvailability.Distinct().ToArray();
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