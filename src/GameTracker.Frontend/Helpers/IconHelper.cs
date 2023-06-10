using BlazorBootstrap;
using GameTracker.Models.Enums;

namespace GameTracker.Frontend.Helpers
{
    public static class IconHelper
    {
        public static IconName GetIconEnum(string iconName, IconName defaultIcon)
        {
            if (string.IsNullOrEmpty(iconName))
            {
                return defaultIcon;
            }

            return Enum.Parse<IconName>(iconName);
        }

        public static IconName GetIconFromLinkType(LinkType linkType)
        {
            return linkType switch
            {
                LinkType.Discord => IconName.Discord,
                LinkType.Mastodon => IconName.Mastodon,
                LinkType.Reddit => IconName.Reddit,
                LinkType.Steam => IconName.Steam,
                LinkType.Twitch => IconName.Twitch,
                LinkType.Twitter => IconName.Twitter,
                LinkType.YouTube => IconName.Youtube,
                LinkType.Web => IconName.Globe,
                _ => IconName.QuestionCircle
            };
        }
    }
}