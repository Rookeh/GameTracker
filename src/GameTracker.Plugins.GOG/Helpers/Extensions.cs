using GameTracker.Models;
using GameTracker.Models.Constants;
using GameTracker.Plugins.GOG.Models.GOGApi;

namespace GameTracker.Plugins.GOG.Helpers
{
    internal static class Extensions
    {
        public static Platform[] FromContentSystemCompatibility(this ContentSystemCompatibility contentSystemCompatibility)
        {
            var platforms = new List<Platform>();

            if (contentSystemCompatibility.Linux)
            {
                platforms.Add(WellKnownPlatforms.Linux);
            }

            if (contentSystemCompatibility.OSX)
            {
                platforms.Add(WellKnownPlatforms.MacOS);
            }

            if (contentSystemCompatibility.Windows)
            {
                platforms.Add(WellKnownPlatforms.Windows);
            }

            return platforms.ToArray();
        }
    }
}