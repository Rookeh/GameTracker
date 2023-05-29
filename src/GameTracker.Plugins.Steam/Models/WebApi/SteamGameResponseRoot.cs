using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Steam.Models.WebApi
{
    internal class SteamGameResponseRoot
    {
        [JsonPropertyName("response")]
        public SteamGameResponse Response { get; set; }
    }
}