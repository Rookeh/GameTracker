using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.Authentication
{
    public class TokenExchangeRequest
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; }

        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }

        [JsonPropertyName("token_format")]
        public string TokenFormat { get; set; }
    }
}