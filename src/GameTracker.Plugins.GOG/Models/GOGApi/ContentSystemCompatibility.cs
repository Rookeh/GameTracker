using System.Text.Json.Serialization;

namespace GameTracker.Plugins.GOG.Models.GOGApi
{
    public class ContentSystemCompatibility
    {
        [JsonPropertyName("windows")]
        public bool Windows { get; set; }

        [JsonPropertyName("osx")]
        public bool OSX { get; set; }

        [JsonPropertyName("linux")]
        public bool Linux { get; set; }
    }
}