using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Nintendo.Helpers;
using GameTracker.Plugins.Nintendo.Models.EU;

namespace GameTracker.Plugins.Nintendo.Models
{
    public class EUNintendoGame : Game
    {
        private readonly string _description;
        private readonly Genre[] _genres;
        private readonly Image _image;
        private readonly LaunchCommand _launchCommand;
        private readonly DateTime? _releaseDate;
        private readonly Studio? _studio;
        private readonly string _title;
        
        public EUNintendoGame(EUDocument gameDocument)
        {
            _description = gameDocument.ProductCatalogDescription;
            _genres = EUGameHelpers.GenresFromGameCategories(gameDocument.GameCategories);
            _image = EUGameHelpers.ImageFromImageUrl(gameDocument.ImageUrlH2X1);
            _launchCommand = EUGameHelpers.LaunchCommandFromUrl(gameDocument.Url);
            _releaseDate = gameDocument.DatesReleased.Min();
            _studio = EUGameHelpers.StudioFromCopyright(gameDocument.Copyright);
            _title = gameDocument.Title;
        }

        public override ControlScheme[] ControlSchemes => new[] { ControlScheme.Controller } ;

        public override string Description => _description;

        public override GameplayMode[] GameplayModes => Array.Empty<GameplayMode>();

        public override Genre[] Genres => _genres;

        public override Image Image => _image;

        public override DateTime? LastPlayed => null;

        public override LaunchCommand LaunchCommand => _launchCommand;

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override Platform[] Platforms => new[] { Constants.Platforms.NintendoSwitch };

        public override TimeSpan? Playtime => null;

        public override Publisher? Publisher => null;

        public override DateTime? ReleaseDate => _releaseDate;

        public override Review[] Reviews => Array.Empty<Review>();

        public override string StorefrontName => "Nintendo eShop";

        public override Studio? Studio => _studio;

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _title;

        public override Task Preload()
        {
            return Task.CompletedTask;
        }
    }
}