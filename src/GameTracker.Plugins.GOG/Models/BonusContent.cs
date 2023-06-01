using System.Text.Json.Serialization;

namespace GameTracker.Plugins.GOG.Models
{
    public class BonusContent
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("total_size")]
        public int TotalSize { get; set; }

        [JsonPropertyName("files")]
        public File[] Files { get; set; }
    }
}