using GameTracker.Models.Enums;

namespace GameTracker.Models.Constants
{
    public static class WellKnownPlatforms
    {
        public static Platform Windows => new Platform
        {            
            Name = "Windows",
            Description = "Microsoft Windows is a group of several proprietary graphical operating system families developed and marketed by Microsoft.",
            Links = new SocialLink[]
                    {
                        new SocialLink
                        {
                            LinkPlatform = LinkType.Web,
                            LinkTarget = "https://www.microsoft.com/windows"
                        }
                    }
        };

        public static Platform Linux => new Platform
        {
            Name = "Linux",
            Description = "Linux is a family of open-source Unix-like operating systems based on the Linux kernel, an operating system kernel first released on September 17, 1991, by Linus Torvalds.",
            Links = new SocialLink[]
                    {
                        new SocialLink
                        {
                            LinkPlatform = LinkType.Web,
                            LinkTarget = "https://www.linux.org/"
                        }
                    }
        };

        public static Platform MacOS => new Platform
        {
            Name = "macOS",
            Description = "macOS is a Unix operating system developed and marketed by Apple Inc. since 2001. It is the primary operating system for Apple's Mac computers.",
            Links = new SocialLink[]
                    {
                        new SocialLink
                        {
                            LinkPlatform = LinkType.Web,
                            LinkTarget = "https://www.apple.com/macos/"
                        }
                    }
        };
    }
}