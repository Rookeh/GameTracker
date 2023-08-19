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
                    return (int)Math.Round((double)(100 * r.Score) / r.UpperBound);
                }).Average();
            }
        }

        public List<Review> Reviews { get; set; } = new List<Review>();
        public DateTime? ReleaseDate { get; set; }
        public GameplayMode[] GameplayModes { get; set; }
        public Genre[] Genres { get; set; }
        public Image Image { get; set; }
        public string Publisher { get; set; }
        public string Studio { get; set; }
        public string Description { get; set; }

        #region Abstract methods/props

        public abstract Task Preload();

        public abstract ControlScheme[] ControlSchemes { get; }                       
        public abstract DateTime? LastPlayed { get; }
        public abstract LaunchCommand LaunchCommand { get; }
        public abstract MultiplayerAvailability[] MultiplayerAvailability { get; }            
        public abstract Platforms Platforms { get; }
        public abstract TimeSpan? Playtime { get; }        
        public abstract string ProviderName { get; }        
        public abstract string[] Tags { get; }
        public abstract string Title { get; }

        #endregion
    }
}