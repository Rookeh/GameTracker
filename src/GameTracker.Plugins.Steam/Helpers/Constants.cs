using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Helpers
{
    public static class Constants
    {
        public const string ConnectionString = "Data Source=SteamGames.sqlite";
        public static SteamGameDetailsRoot GetDefaultSteamGameDetails()
        {
            return new SteamGameDetailsRoot
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