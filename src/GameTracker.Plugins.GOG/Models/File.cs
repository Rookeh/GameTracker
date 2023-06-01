using System.Text.Json.Serialization;

namespace GameTracker.Plugins.GOG.Models
{
    public class File
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("downlink")]
        public string Downlink { get; set; }
    }
}