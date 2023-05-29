using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Steam.Models.WebApi
{
    public class SteamApp
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