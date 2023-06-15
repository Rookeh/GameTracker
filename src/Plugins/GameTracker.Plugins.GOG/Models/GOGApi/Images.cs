using System.Text.Json.Serialization;

namespace GameTracker.Plugins.GOG.Models.GOGApi
{
    public class Images
    {
        [JsonPropertyName("background")]
        public string Background { get; set; }

        [JsonPropertyName("logo")]
        public string Logo { get; set; }

        [JsonPropertyName("logo2x")]
        public string Logo2x { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("sidebarIcon")]
        public string SidebarIcon { get; set; }

        [JsonPropertyName("sidebarIcon2x")]
        public string SidebarIcon2x { get; set; }

        [JsonPropertyName("menuNotificationAv")]
        public string MenuNotificationAv { get; set; }

        [JsonPropertyName("menuNotificationAv2")]
        public string MenuNotificationAv2 { get; set; }
    }
}