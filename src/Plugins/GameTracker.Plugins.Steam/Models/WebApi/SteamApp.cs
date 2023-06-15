using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("GameTracker.Plugins.Steam.Tests")]
namespace GameTracker.Plugins.Steam.Models.WebApi
{
    internal class SteamApp
    {
        [JsonPropertyName("appid")]
        public int AppId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("last_modified")]
        public int LastModified { get; set; }

        [JsonPropertyName("price_change_number")]
        public int PriceChangeNumber { get; set; }
    }
}