using GameTracker.Models.Enums;

namespace GameTracker.Models
{
    public abstract class Game
    {
        public int PlatformId { get; set; }

        public double? AverageCriticalReview
        {
            get
            {
                if (!Reviews.Any())
                {
                    return null;
                }

                return Reviews.Select(r =>
                {
                    return (int)Math.Round((double)(100 * r.Score) / r.Critic.UpperBound);
                }).Average();
            }
        }

        public string StudioName => Studio?.Name ?? string.Empty;
        public string PublisherName => Publisher?.Name ?? string.Empty;
        public string GenreString => Genres.Any() ? string.Join(',', Genres) : string.Empty;

        public abstract Task Preload();
        public abstract TimeSpan? Playtime { get; }
        public abstract DateTime? LastPlayed { get; }
        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract Genre[] Genres { get; }
        public abstract Image Image { get; }
        public abstract LaunchCommand LaunchCommand { get; }
        public abstract MultiplayerAvailability[] MultiplayerAvailability { get; }
        public abstract GameplayMode[] GameplayModes { get; }        
        public abstract Platform[] Platforms { get; }
        public abstract Publisher? Publisher { get; }
        public abstract DateTime? ReleaseDate { get; }
        public abstract Review[] Reviews { get; }
        public abstract Studio? Studio { get; }
        public abstract string[] Tags { get; }
    }
}