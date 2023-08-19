using GameTracker.Models.Enums;
using GameTracker.Plugins.GOG.Models.GOGApi;

namespace GameTracker.Plugins.GOG.Helpers
{
    internal static class Extensions
    {
        public static Platforms FromContentSystemCompatibility(this ContentSystemCompatibility contentSystemCompatibility)
        {
            var platforms = Platforms.None;

            if (contentSystemCompatibility.Windows)
            {
                platforms |= Platforms.Windows;
            }

            if (contentSystemCompatibility.OSX)
            {
                platforms |= Platforms.MacOS;
            }

            if (contentSystemCompatibility.Linux)
            {
                platforms |= Platforms.Linux;
            }

            return platforms;
        }
    }
}