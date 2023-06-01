using System.Text.Json.Serialization;

namespace GameTracker.Plugins.GOG.Models
{
    public class Installer
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("os")]
        public string OS { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("language_full")]
        public string LanguageFull { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("total_size")]
        public long TotalSize { get; set; }

        [JsonPropertyName("files")]
        public File[] Files { get; set; }
    }
}