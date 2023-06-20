using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.GraphQL.StoreInfo
{
    public class Price
    {
        [JsonPropertyName("__typename")]
        public string TypeName { get; set; }

        [JsonPropertyName("basePrice")]
        public string BasePrice { get; set; }

        [JsonPropertyName("discountedPrice")]
        public string DiscountedPrice { get; set; }

        [JsonPropertyName("serviceBranding")]
        public string[] ServiceBranding { get; set; }
    }
}