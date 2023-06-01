using System.Text.Json.Serialization;

namespace GameTracker.Plugins.GOG.Models
{
    public class Links
    {
        [JsonPropertyName("purchase_link")]
        public string PurchaseLink { get; set; }

        [JsonPropertyName("product_card")]
        public string ProductCard { get; set; }

        [JsonPropertyName("support")]
        public string Support { get; set; }

        [JsonPropertyName("forum")]
        public string Forum { get; set; }
    }
}