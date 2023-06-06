using BlazorBootstrap;

namespace GameTracker.Frontend.Helpers
{
    public static class IconHelper
    {
        public static IconName GetIconEnum(string iconName)
        {
            return Enum.Parse<IconName>(iconName);
        }
    }
}