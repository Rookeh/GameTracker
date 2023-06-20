using GameTracker.Models.Enums;

namespace GameTracker.Plugins.PlayStation.Helpers
{
    internal static class PlayStationGameHelpers
    {
        internal static IEnumerable<Genre> GenresFromGenreStrings(string[] genreStrings)
        {
            foreach (var genre in genreStrings)
            {
                if (Constants.Mappings.PSNGenreMappings.ContainsKey(genre))
                {
                    yield return Constants.Mappings.PSNGenreMappings[genre];
                }
            }
        }
    }
}