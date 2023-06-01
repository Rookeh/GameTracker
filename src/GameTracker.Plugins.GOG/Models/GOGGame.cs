using GameTracker.Models;
using GameTracker.Models.Enums;

namespace GameTracker.Plugins.GOG.Models
{
    public class GOGGame : Game
    {
        private readonly string _description;
        private readonly string _imageUrl;
        private readonly string _launchCommand;
        private readonly Platform[] _platforms;
        private readonly DateTime _releaseDate;
        private readonly string _title;

        public GOGGame(string description, string imageUrl, string launchCommand, Platform[] platforms,
            DateTime releaseDate, string title)
        {
            _description = description;
            _imageUrl = imageUrl;
            _launchCommand = launchCommand;
            _platforms = platforms;
            _releaseDate = releaseDate;
            _title = title;
        }

        public override string Description => _description;

        public override Genre[] Genres => Array.Empty<Genre>();

        public override string Image => _imageUrl;

        public override string LaunchCommand => _launchCommand;

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override MultiplayerMode[] MultiplayerModes => Array.Empty<MultiplayerMode>();

        public override Platform[] Platforms => _platforms;

        public override TimeSpan Playtime => TimeSpan.Zero;

        public override Publisher Publisher => new Publisher { Name = "Unknown", Description = "Unknown" };

        public override DateTime ReleaseDate => _releaseDate;

        public override Review[] Reviews => Array.Empty<Review>();

        public override Studio Studio => new Studio { Name = "Unknown", Description = "Unknown" };

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _title;        
    }
}