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
                ["Adventure"] = GenreEnum.Adventure,
                ["Casual"] = GenreEnum.Casual,
                ["Early Access"] = GenreEnum.EarlyAccess,
                ["FreeToPlay"] = GenreEnum.FreeToPlay,
                ["Indie"] = GenreEnum.Indie,
                ["Strategy"] = GenreEnum.Strategy,
                ["RPG"] = GenreEnum.RPG,
                ["Racing"] = GenreEnum.Racing,
                ["Simulation"] = GenreEnum.Simulation,
                ["Massively Multiplayer"] = GenreEnum.MMO,
                ["VR"] = GenreEnum.VR
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

            internal static Dictionary<string, GameplayMode> SteamMultiplayerModeMappings => new Dictionary<string, GameplayMode>
            {
                ["Online PvP"] = GameplayMode.PvP,
                ["Online Co-op"] = GameplayMode.CoOp,
                ["MMO"] = GameplayMode.PvE,
                ["Single-player"] = GameplayMode.Singleplayer,
                ["Shared/Split Screen PvP"] = GameplayMode.PvP,
                ["Shared/Split Screen Co-op"] = GameplayMode.CoOp,
                ["LAN PvP"] = GameplayMode.PvP,
                ["LAN Co-op"] = GameplayMode.CoOp
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
                            Date = null,
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