using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("GameTracker.Plugins.Steam.Tests")]
namespace GameTracker.Plugins.Steam.Models.WebApi
{
    public class SteamGameResponse
    {
        [JsonPropertyName("game_count")]
        public int GameCount { get; set; }

        [JsonPropertyName("games")]
        public SteamGameDto[] Games { get; set; }
    }
}