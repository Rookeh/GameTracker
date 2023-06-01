using System.Text.Json.Serialization;

namespace GameTracker.Plugins.GOG.Models
{
    public class GameDetails
    {
        [JsonIgnore]
        public bool IsDefaultValue { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("purchase_link")]
        public string PurchaseLink { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("content_system_compatibility")]
        public ContentSystemCompatibility ContentSystemCompatibility { get; set; }

        [JsonPropertyName("links")]
        public Links Links { get; set; }

        [JsonPropertyName("in_development")]
        public InDevelopment InDevelopment { get; set; }

        [JsonPropertyName("is_secret")]
        public bool IsSecret { get; set; }

        [JsonPropertyName("is_installable")]
        public bool IsInstallable { get; set; }

        [JsonPropertyName("game_type")]
        public string GameType { get; set; }

        [JsonPropertyName("is_pre_order")]
        public bool IsPreOrder { get; set; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("images")]
        public Images Images { get; set; }

        [JsonPropertyName("downloads")]
        public Downloads Downloads { get; set; }

        [JsonPropertyName("description")]
        public Description Description { get; set; }

        [JsonPropertyName("changelog")]
        public string Changelog { get; set; }
    }
}
