using GameTracker.Models.Enums;

namespace GameTracker.Models
{
    public abstract class Game
    {
        public int PlatformId { get; set; }
        public abstract TimeSpan Playtime { get; }
        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract Genre[] Genres { get; }
        public abstract string Image { get; }
        public abstract string LaunchCommand { get; }
        public abstract MultiplayerAvailability[] MultiplayerAvailability { get; }
        public abstract MultiplayerMode[] MultiplayerModes { get; }        
        public abstract Platform[] Platforms { get; }        
        public abstract Publisher Publisher { get; }
        public abstract DateTime ReleaseDate { get; }
        public abstract Review[] Reviews { get; }
        public abstract Studio Studio { get; }
        public abstract string[] Tags { get; }     
    }
}