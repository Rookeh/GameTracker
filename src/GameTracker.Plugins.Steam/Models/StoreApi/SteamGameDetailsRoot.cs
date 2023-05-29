
using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Steam.Models.StoreApi
{
    public class SteamGameDetailsRoot
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public SteamGameDetails Details { get; set; }
    }
}