using GameTracker.Models.Enums;

namespace GameTracker.Models
{
    public class SocialLink
    {
        public LinkType LinkPlatform { get; set; }
        public string LinkTarget { get; set; }
    }
}