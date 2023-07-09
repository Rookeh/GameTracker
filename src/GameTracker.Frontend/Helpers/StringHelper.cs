namespace GameTracker.Frontend.Helpers
{
    public static class StringHelper
    {
        public static string[] ToSanitizedKeywordArray(this string input)
        {
            var unpunctuated = new string(input.Where(c => !char.IsPunctuation(c)).ToArray()).ToLower();
            return unpunctuated.Split(' ').Select(s => s.Trim()).ToArray();
        }
    }
}