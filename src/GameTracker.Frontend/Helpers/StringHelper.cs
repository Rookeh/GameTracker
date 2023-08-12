namespace GameTracker.Frontend.Helpers
{
    public static class StringHelper
    {
        public static string[] ToSanitizedKeywordArray(this string input)
        {
            var unpunctuated = new string(input.Replace('-', ' ').Where(c => char.IsWhiteSpace(c) || char.IsLetterOrDigit(c)).ToArray()).ToLower();
            return unpunctuated.Split(' ').Select(s => s.Trim()).Distinct().ToArray();
        }
    }
}