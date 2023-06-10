using System.Drawing;

namespace GameTracker.Frontend.Helpers
{
    public static class ColourHelper
    {
        private static Random RNG = new Random();

        public static class ChartColours
        {
            public static Color Primary = ColorTranslator.FromHtml("#2276DD");
            public static Color Secondary = ColorTranslator.FromHtml("#DD8922");
            public static Color Tertiary = ColorTranslator.FromHtml("#4D19E6");
            public static Color Quaternary = ColorTranslator.FromHtml("#B2E619");
            public static Color Quinary = ColorTranslator.FromHtml("#E51A21");
            public static Color Senary = ColorTranslator.FromHtml("#1AE5DE");
            public static Color Neutral = ColorTranslator.FromHtml("#7D7F82");
        }

        public static Color GetRandomColour()
        {
            return Color.FromArgb(RNG.Next(0, 256), RNG.Next(0, 256), RNG.Next(0, 256));
        }
    }
}