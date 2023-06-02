using System.Text.Json.Serialization;

namespace GameTracker.Plugins.GOG.Models.GOGApi
{
    public class Description
    {
        [JsonPropertyName("lead")]
        public string Lead { get; set; }

        [JsonPropertyName("full")]
        public string Full { get; set; }

        [JsonPropertyName("whats_cool_about_it")]
        public string WhatsCoolAboutIt { get; set; }
    }
}