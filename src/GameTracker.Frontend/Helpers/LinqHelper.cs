using GameTracker.Models;

namespace GameTracker.Frontend.Helpers
{
    public static class LinqHelper
    {
        #region Filters

        public static Func<IGrouping<string, Game>, bool> TitleFilter(string? filter) => g => string.IsNullOrEmpty(filter) ? true : g.Key.ToLower().Contains(filter.ToLower());
        public static Func<IGrouping<string, Game>, bool> PublisherFilter(string? filter) => g => string.IsNullOrEmpty(filter) ? true : g.Any(g2 => g2.PublisherName.ToLower().Contains(filter.ToLower()));
        public static Func<IGrouping<string, Game>, bool> ProviderFilter(string? filter) => g => string.IsNullOrEmpty(filter) ? true : g.Any(g2 => g2.ProviderName.ToLower().Contains(filter.ToLower()));
        public static Func<IGrouping<string, Game>, bool> StudioFilter(string? filter) => g => string.IsNullOrEmpty(filter) ? true : g.Any(g2 => g2.StudioName.ToLower().Contains(filter.ToLower()));
        public static Func<IGrouping<string, Game>, bool> GenreFilter(string? filter) => g => string.IsNullOrEmpty(filter) ? true : g.Any(g2 => g2.Genres != null && g2.Genres.Any(genre => genre.ToString().ToLower() == filter.ToLower()));
        public static Func<IGrouping<string, Game>, bool> ReleaseDateFilter(DateOnly? filter) => g => filter == null ? true : g.Any(g2 => g2.ReleaseDate != null && DateOnly.FromDateTime(g2.ReleaseDate ?? DateTime.UnixEpoch) >= filter);
        public static Func<IGrouping<string, Game>, bool> LastPlayedFilter(DateTime? filter) => g => filter == null ? true : g.Any(g2 => g2.LastPlayed != null && g2.LastPlayed > DateTime.UnixEpoch.AddDays(5) && g2.LastPlayed <= filter);
        public static Func<IGrouping<string, Game>, bool> MinPlaytimeFilter(TimeSpan? filter) => g => filter == null ? true : g.Any(g2 => g2.Playtime != null && g2.Playtime >= filter);
        public static Func<IGrouping<string, Game>, bool> MaxPlaytimeFilter(TimeSpan? filter) => g => filter == null ? true : g.Any(g2 => g2.Playtime != null && g2.Playtime <= filter);
        public static Func<IGrouping<string, Game>, bool> ReviewScoreFilter(int? filter) => g => filter == null ? true : g.Any(g2 => g2.AverageCriticalReview >= filter);

        #endregion

        #region Selectors

        public static Func<IGrouping<string, Game>, object?> AverageReviewSelector => g => g.FirstOrDefault(g2 => g2.AverageCriticalReview.HasValue)?.AverageCriticalReview;
        public static Func<IGrouping<string, Game>, object?> LastPlayedSelector => g => g.FirstOrDefault(g2 => g2.LastPlayed.HasValue)?.LastPlayed;
        public static Func<IGrouping<string, Game>, object?> PlaytimeSelector => g => g.FirstOrDefault(g2 => g2.Playtime.HasValue)?.Playtime;
        public static Func<IGrouping<string, Game>, string> PublisherSelector => g => g.FirstOrDefault(g2 => !string.IsNullOrEmpty(g2.PublisherName))?.PublisherName;
        public static Func<IGrouping<string, Game>, object?> ReleaseDateSelector => g => g.FirstOrDefault(g2 => g2.ReleaseDate.HasValue)?.ReleaseDate;
        public static Func<IGrouping<string, Game>, string> StudioSelector => g => g.FirstOrDefault(g2 => !string.IsNullOrEmpty(g2.StudioName))?.StudioName;
        public static Func<IGrouping<string, Game>, string> TitleSelector => g => g.FirstOrDefault(g2 => !string.IsNullOrEmpty(g2.Title))?.Title;

        #endregion
    }
}