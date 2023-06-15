using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Xbox.Models.OpenXBL
{
    public class Stats
    {
        [JsonPropertyName("sourceVersion")]
        public int SourceVersion { get; set; }
    }
}