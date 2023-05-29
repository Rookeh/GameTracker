using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Steam.Models.WebApi
{
    public class SteamAppResponse
    {
        [JsonPropertyName("apps")]
        public SteamApp[] Apps { get; set; }

        [JsonPropertyName("have_more_results")]
        public bool HasMoreResults { get; set; }

        [JsonPropertyName("last_appid")]
        public int LastAppId { get; set; }
    }
}