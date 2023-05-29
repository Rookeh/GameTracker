using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Steam.Models.StoreApi
{
    public class MetacriticScore
    {
        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}