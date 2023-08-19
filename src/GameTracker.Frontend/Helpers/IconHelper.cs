using BlazorBootstrap;
using GameTracker.Frontend.Models;
using GameTracker.Models.Enums;

using PlatformEnum = GameTracker.Models.Enums.Platforms;

namespace GameTracker.Frontend.Helpers
{
    public static class IconHelper
    {
        public static IconName GetIconFromPlatform(PlatformEnum platform)
        {           
            switch (platform)
            {
                case PlatformEnum.Windows:
                    return IconName.Windows;
                case PlatformEnum.MacOS:
                    return IconName.Apple;
                case PlatformEnum.Linux:
                    return IconName.Ubuntu;
                case PlatformEnum.NintendoSwitch:
                    return IconName.NintendoSwitch;
                case PlatformEnum.PlayStation3:
                case PlatformEnum.PlayStation4:
                case PlatformEnum.PlayStation5:
                    return IconName.Playstation;
                case PlatformEnum.Xbox360:
                case PlatformEnum.XboxOne:
                case PlatformEnum.XboxSeries:
                    return IconName.Xbox;
                default: 
                    return IconName.QuestionCircle;
            }
        }

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

        public static IconName GetSortIcon(bool ascending, SortField sortField)
        {
            return (ascending, sortField) switch
            {
                (true, SortField.AverageReview) => IconName.SortNumericUp,
                (true, SortField.LastPlayed) => IconName.SortNumericUp,
                (true, SortField.PlayTime) => IconName.SortNumericUp,
                (true, SortField.Publisher) => IconName.SortAlphaUp,
                (true, SortField.ReleaseDate) => IconName.SortNumericUp,
                (true, SortField.StudioName) => IconName.SortAlphaUp,
                (true, SortField.Title) => IconName.SortAlphaUp,
                (false, SortField.AverageReview) => IconName.SortNumericDown,
                (false, SortField.LastPlayed) => IconName.SortNumericDown,
                (false, SortField.PlayTime) => IconName.SortNumericDown,
                (false, SortField.Publisher) => IconName.SortAlphaDown,
                (false, SortField.ReleaseDate) => IconName.SortNumericDown,
                (false, SortField.StudioName) => IconName.SortAlphaDown,
                (false, SortField.Title) => IconName.SortAlphaDown,
                _ => IconName.Question
            };
        }
    }
}