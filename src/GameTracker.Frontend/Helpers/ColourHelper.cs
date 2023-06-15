using System.Drawing;

namespace GameTracker.Frontend.Helpers
{
    public static class ColourHelper
    {
        private static Random RNG = new Random();

        public static class ChartColours
        {
            public static Color Neutral = ColorTranslator.FromHtml("#7D7F82");
        }

        public static Color GetRandomColour()
        {
            return Color.FromArgb(RNG.Next(0, 256), RNG.Next(0, 256), RNG.Next(0, 256));
        }
    }
}