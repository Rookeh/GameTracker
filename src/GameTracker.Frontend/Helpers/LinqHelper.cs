using GameTracker.Models;

namespace GameTracker.Frontend.Helpers
{
    public static class LinqHelper
    {
        #region Filters

        public static Func<Game, bool> TitleFilter(string? filter) => g => string.IsNullOrEmpty(filter) ? true : g.Title.ToLower().Contains(filter.ToLower());

        public static Func<Game, bool> PublisherFilter(string? filter) => g => string.IsNullOrEmpty(filter) ? true : g.PublisherName.ToLower().Contains(filter.ToLower());

        public static Func<Game, bool> ProviderFilter(string? filter) => g => string.IsNullOrEmpty(filter) ? true : g.ProviderName.ToLower().Contains(filter.ToLower());

        public static Func<Game, bool> StudioFilter(string? filter) => g => string.IsNullOrEmpty(filter) ? true : g.StudioName.ToLower().Contains(filter.ToLower());

        public static Func<Game, bool> GenreFilter(string? filter) => g => string.IsNullOrEmpty(filter) ? true : g.Genres != null && g.Genres.Any(genre => genre.ToString().ToLower() == filter.ToLower());

        public static Func<Game, bool> ReleaseDateFilter(DateOnly? filter) => g => filter == null ? true : g.ReleaseDate != null && DateOnly.FromDateTime(g.ReleaseDate ?? DateTime.UnixEpoch) >= filter;

        public static Func<Game, bool> LastPlayedFilter(DateTime? filter) => g => filter == null ? true : g.LastPlayed != null && g.LastPlayed > DateTime.UnixEpoch.AddDays(5) && g.LastPlayed <= filter;

        public static Func<Game, bool> MinPlaytimeFilter(TimeSpan? filter) => g => filter == null ? true : g.Playtime != null && g.Playtime >= filter;

        public static Func<Game, bool> MaxPlaytimeFilter(TimeSpan? filter) => g => filter == null ? true : g.Playtime != null && g.Playtime <= filter;

        public static Func<Game, bool> ReviewScoreFilter(int? filter) => g => filter == null ? true : g.AverageCriticalReview >= filter;

        #endregion

        #region Selectors

        public static Func<Game, object?> AverageReviewSelector => g => g.AverageCriticalReview;

        public static Func<Game, object?> LastPlayedSelector => g => g.LastPlayed;

        public static Func<Game, object?> PlaytimeSelector => g => g.Playtime;

        public static Func<Game, string> PublisherSelector => g => g.PublisherName;

        public static Func<Game, object?> ReleaseDateSelector => g => g.ReleaseDate;

        public static Func<Game, string> StudioSelector => g => g.StudioName;        

        public static Func<Game, string> TitleSelector => g => g.Title;

        #endregion
    }
}