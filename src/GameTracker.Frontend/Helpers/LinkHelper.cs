using GameTracker.Models;
using GameTracker.Models.Enums;

namespace GameTracker.Frontend.Helpers
{
    public static class LinkHelper
    {
        public static string GetLinkUri(SocialLink link)
        {
            return link.LinkPlatform switch
            {
                LinkType.Discord => $"https://discord.com/servers/{link.LinkTarget}",
                LinkType.Mastodon => $"https://mastodon.social/{link.LinkTarget}",
                LinkType.Patreon => $"https://www.patreon.com/{link.LinkTarget}",
                LinkType.Reddit => $"https://reddit.com/r/{link.LinkTarget}",
                LinkType.Steam => $"https://store.steampowered.com/{link.LinkTarget}",
                LinkType.Twitch => $"https://www.twitch.tv/{link.LinkTarget}",
                LinkType.Twitter => $"https://twitter.com/{(link.LinkTarget.Contains('@') ? link.LinkTarget.Replace("@", string.Empty) : link.LinkTarget)}",
                LinkType.YouTube => $"https://www.youtube.com/{link.LinkTarget}",
                _ => link.LinkTarget
            };
        }
    }
}