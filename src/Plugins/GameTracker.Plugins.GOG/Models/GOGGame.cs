using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.GOG.Helpers;
using GameTracker.Plugins.GOG.Models.GOGApi;

namespace GameTracker.Plugins.GOG.Models
{
    public class GOGGame : Game
    {
        private readonly string _description;
        private readonly string _imageUrl;
        private readonly Platform[] _platforms;
        private readonly DateTime? _releaseDate;
        private readonly string _title;

        internal GOGGame(GameDetails gameDetails)
        {
            PlatformId = gameDetails.Id;
            _description = gameDetails.Description?.Full ?? string.Empty;
            _imageUrl = gameDetails.Images?.Logo2x ?? "img\\placeholder.png";
            _platforms = gameDetails.ContentSystemCompatibility.FromContentSystemCompatibility();
            _releaseDate = string.IsNullOrEmpty(gameDetails.ReleaseDate)
                ? null
                : DateTime.Parse(gameDetails.ReleaseDate);
            _title = gameDetails.Title;
        }

        public override Task Preload()
        {
            return Task.CompletedTask;
        }

        public override ControlScheme[] ControlSchemes => Array.Empty<ControlScheme>() ;

        public override string Description => _description;

        public override GameplayMode[] GameplayModes => Array.Empty<GameplayMode>();

        public override Genre[] Genres => Array.Empty<Genre>();

        public override Image Image => new Image
        {
            Url = _imageUrl,
            Width = 200,
            Height = 120
        };

        public override LaunchCommand LaunchCommand => new LaunchCommand
        {
            NewTab = false,
            Icon = "PcDisplay",
            Text = "Launch via GOG Galaxy",
            Url = $"goggalaxy://openGameView/{PlatformId}"
        };

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();        

        public override DateTime? LastPlayed => null;

        public override Platform[] Platforms => _platforms;

        public override TimeSpan? Playtime => null;

        public override Publisher? Publisher => null;

        public override DateTime? ReleaseDate => _releaseDate;

        public override Review[] Reviews => Array.Empty<Review>();

        public override string ProviderName => "GOG";

        public override Studio? Studio => null;

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _title;

    }
}