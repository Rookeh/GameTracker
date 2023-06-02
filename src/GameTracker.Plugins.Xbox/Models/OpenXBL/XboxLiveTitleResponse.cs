using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Xbox.Models.OpenXBL
{
    public class XboxLiveTitleResponse
    {
        [JsonPropertyName("xuid")]
        public string XUID { get; set; }

        [JsonPropertyName("titles")]
        public Title[] Titles { get; set; }
    }
}