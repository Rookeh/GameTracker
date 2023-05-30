
using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Steam.Models.StoreApi
{
    public class SteamGameDetails
    {
        [JsonIgnore]
        public bool IsDefaultValue { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("steam_appid")]
        public int AppId { get; set; }

        [JsonPropertyName("is_free")]
        public bool IsFree { get; set; }

        [JsonPropertyName("detailed_description")]
        public string Description { get; set; }

        [JsonPropertyName("about_the_game")]
        public string About { get; set; }

        [JsonPropertyName("short_description")]
        public string ShortDescription { get; set; }

        [JsonPropertyName("supported_languages")]
        public string Languages { get; set; }

        [JsonPropertyName("header_image")]
        public string HeaderImage { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("developers")]
        public string[] Developers { get; set; }

        [JsonPropertyName("publishers")]
        public string[] Publishers { get; set; }

        [JsonPropertyName("platforms")]
        public Platforms Platforms { get; set; }

        [JsonPropertyName("categories")]
        public Category[] Categories { get; set; }

        [JsonPropertyName("genres")]
        public Genre[] Genres { get; set; }

        [JsonPropertyName("release_date")]
        public ReleaseDate ReleaseDate { get; set; }

        [JsonPropertyName("metacritic")]
        public MetacriticScore Metacritic { get; set; }
    }
}