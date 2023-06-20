using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.GraphQL.StoreInfo
{
    public class ProductRetrieve
    {
        [JsonPropertyName("__typename")]
        public string TypeName { get; set; }

        [JsonPropertyName("contentRating")]
        public ContentRating ContentRating { get; set; }

        [JsonPropertyName("edition")]
        public Edition Edition { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("localizedGenres")]
        public LocalizedGenre[] LocalizedGenres { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("npTitleId")]
        public string NpTitleId { get; set; }

        [JsonPropertyName("skus")]
        public SKU[] SKUs { get; set; }

        [JsonPropertyName("webctas")]
        public WebCTA[] WebCTAs { get; set; }
    }
}