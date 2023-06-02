using GameTracker.Models.Enums;
using GameTracker.Plugins.Steam.Models.StoreApi;
using Genre = GameTracker.Plugins.Steam.Models.StoreApi.Genre;
using GenreEnum = GameTracker.Models.Enums.Genre;

namespace GameTracker.Plugins.Steam.Helpers
{
    internal static class Constants
    {
        internal static class SQLite
        {
            internal const string ConnectionString = "Data Source=SteamGames.sqlite";
        }

        internal static class SteamCategoryMappings
        {
            internal static Dictionary<string, GenreEnum> SteamGenreMappings => new Dictionary<string, GenreEnum>
            {
                ["Action"] = GenreEnum.Action,
                ["Adventure"] = GenreEnum.ActionAdventure,
                ["Casual"] = GenreEnum.Casual,
                ["Early Access"] = GenreEnum.EarlyAccess,
                ["FreeToPlay"] = GenreEnum.FreeToPlay,
                ["Indie"] = GenreEnum.Indie,
                ["Strategy"] = GenreEnum.Strategy,
                ["RPG"] = GenreEnum.RPG,
                ["Racing"] = GenreEnum.Racing,
                ["Simulation"] = GenreEnum.Simulation,
                ["Massively Multiplayer"] = GenreEnum.MMO
            };

            internal static Dictionary<string, MultiplayerAvailability> SteamMultiplayerAvailMappings => new Dictionary<string, MultiplayerAvailability>
            {
                ["Online PvP"] = MultiplayerAvailability.Online,
                ["Remote Play Together"] = MultiplayerAvailability.Online,
                ["Cross-Platform Multiplayer"] = MultiplayerAvailability.Online,
                ["Online Co-op"] = MultiplayerAvailability.Online,
                ["MMO"] = MultiplayerAvailability.Always,
                ["Single-player"] = MultiplayerAvailability.None,
                ["Shared/Split Screen PvP"] = MultiplayerAvailability.Local,
                ["Shared/Split Screen Co-op"] = MultiplayerAvailability.Local,
                ["Shared/Split Screen"] = MultiplayerAvailability.Local,
                ["LAN PvP"] = MultiplayerAvailability.LocalNetwork,
                ["LAN Co-op"] = MultiplayerAvailability.LocalNetwork
            };

            internal static Dictionary<string, MultiplayerMode> SteamMultiplayerModeMappings => new Dictionary<string, MultiplayerMode>
            {
                ["Online PvP"] = MultiplayerMode.PvP,
                ["Online Co-op"] = MultiplayerMode.CoOp,
                ["MMO"] = MultiplayerMode.PvE,
                ["Single-player"] = MultiplayerMode.None,
                ["Shared/Split Screen PvP"] = MultiplayerMode.PvP,
                ["Shared/Split Screen Co-op"] = MultiplayerMode.CoOp,
                ["LAN PvP"] = MultiplayerMode.PvP,
                ["LAN Co-op"] = MultiplayerMode.CoOp
            };
        }

        internal static class DefaultValues
        {
            internal static SteamGameDetailsRoot DefaultSteamGameDetails =>
                new SteamGameDetailsRoot
                {
                    Details = new SteamGameDetails
                    {
                        IsDefaultValue = true,
                        AppId = 0,
                        About = "Pending...",
                        Categories = Array.Empty<Category>(),
                        Description = "Pending...",
                        Developers = Array.Empty<string>(),
                        Genres = Array.Empty<Genre>(),
                        HeaderImage = "img\\placeholder.png",
                        IsFree = false,
                        Languages = string.Empty,
                        Metacritic = new MetacriticScore
                        {
                            Score = 0,
                            Url = string.Empty,
                        },
                        Name = "Pending...",
                        Platforms = new Platforms
                        {
                            Linux = false,
                            Windows = false,
                            Mac = false
                        },
                        Publishers = Array.Empty<string>(),
                        ReleaseDate = new ReleaseDate
                        {
                            Date = DateTime.MaxValue.ToString(),
                            Unreleased = true
                        },
                        ShortDescription = "Pending...",
                        Type = "Pending...",
                        Website = "https://store.steampowered.com/"
                    }
                };
        }
    }
}