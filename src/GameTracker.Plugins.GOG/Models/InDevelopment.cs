using System.Text.Json.Serialization;

namespace GameTracker.Plugins.GOG.Models
{
    public class InDevelopment
    {
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("until")]
        public object Until { get; set; }
    }

}