using System.Text.Json.Serialization;

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