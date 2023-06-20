using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.GraphQL.StoreInfo
{
    public class Data
    {
        [JsonPropertyName("productRetrieve")]
        public ProductRetrieve ProductRetrieve { get; set; }
    }
}