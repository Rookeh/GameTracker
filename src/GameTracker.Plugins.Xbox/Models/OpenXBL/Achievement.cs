using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Xbox.Models.OpenXBL
{
    public class Achievement
    {
        [JsonPropertyName("currentAchievements")]
        public int CurrentAchievements { get; set; }

        [JsonPropertyName("totalAchievements")]
        public int TotalAchievements { get; set; }

        [JsonPropertyName("currentGamerscore")]
        public int CurrentGamerscore { get; set; }

        [JsonPropertyName("totalGamerscore")]
        public int TotalGamerscore { get; set; }

        [JsonPropertyName("progressPercentage")]
        public int ProgressPercentage { get; set; }

        [JsonPropertyName("sourceVersion")]
        public int SourceVersion { get; set; }
    }
}