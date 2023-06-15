using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly:InternalsVisibleTo("GameTracker.Plugins.Steam.Tests")]
namespace GameTracker.Plugins.Steam.Models.WebApi
{
    internal class SteamAppResponse
    {
        [JsonPropertyName("apps")]
        public SteamApp[] Apps { get; set; }

        [JsonPropertyName("have_more_results")]
        public bool HasMoreResults { get; set; }

        [JsonPropertyName("last_appid")]
        public int LastAppId { get; set; }
    }
}