using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Steam.Models.WebApi
{
    public class SteamAppResponseRoot
    {
        [JsonPropertyName("response")]
        public SteamAppResponse Response { get; set; }
    }
}