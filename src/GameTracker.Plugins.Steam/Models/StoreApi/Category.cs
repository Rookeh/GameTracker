
using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Steam.Models.StoreApi
{
    public class Category
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}