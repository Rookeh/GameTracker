﻿using System.Text.Json.Serialization;

namespace GameTracker.Plugins.PlayStation.Models.Authentication
{
    public class TokenExchangeResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("refresh_token_expires_in")]
        public int RefreshTokenExpiresIn { get; set; }
    }
}