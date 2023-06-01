using System.Text.Json.Serialization;

namespace GameTracker.Plugins.GOG.Models
{
    public class Downloads
    {
        [JsonPropertyName("installers")]
        public Installer[] Installers { get; set; }

        [JsonPropertyName("patches")]
        public object[] Patches { get; set; }

        [JsonPropertyName("language_packs")]
        public object[] LanguagePacks { get; set; }

        [JsonPropertyName("bonus_content")]
        public BonusContent[] BonusContent { get; set; }
    }
}