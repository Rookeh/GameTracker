using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.GraphQL.GameLibrary
{
    public class Game
    {
        [JsonPropertyName("conceptId")]
        public string ConceptId { get; set; }

        [JsonPropertyName("entitlementId")]
        public string EntitlementId { get; set; }

        [JsonPropertyName("image")]
        public Image Image { get; set; }

        [JsonPropertyName("isActive")]
        public bool? IsActive { get; set; }

        [JsonPropertyName("lastPlayedDateTime")]
        public DateTime LastPlayedDateTime { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("platform")]
        public string Platform { get; set; }

        [JsonPropertyName("productId")]
        public string ProductId { get; set; }

        [JsonPropertyName("subscriptionService")]
        public string SubscriptionService { get; set; }

        [JsonPropertyName("titleId")]
        public string TitleId { get; set; }
    }
}