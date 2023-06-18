using EpicGamesStoreNET.Models;
using GameTracker.Models;
using GameTracker.Models.Constants;
using GameTracker.Models.Enums;
using System.Globalization;

namespace GameTracker.Plugins.EpicGames.Models
{
    public class EpicGame : Game
    {
        private readonly string _description;
        private readonly string _imageUrl;
        private readonly string _publisherName;
        private readonly string? _studioName; 
        private readonly DateTime _releaseDate;
        private readonly string _title;
        private readonly string _namespace;
        private readonly string _urlSlug;

        internal EpicGame(Element element)
        {
            var hexBytes = Convert.FromHexString(element.Id);
            PlatformId = BitConverter.ToInt32(hexBytes);
            _description = element.Description;
            _imageUrl = element.KeyImages?.FirstOrDefault(i => i.Type == "OfferImageWide")?.Url ?? "img//placeholder.png";
            _namespace = element.Namespace;
            _publisherName = element.Seller.Name;
            _releaseDate = element.EffectiveDate;                        
            _studioName = element.CustomAttributes.FirstOrDefault(ca => ca.Key == "developerName")?.Value ?? null;
            _title = element.Title;
            _urlSlug = element.UrlSlug;
        }

        public override Task Preload()
        {
            return Task.CompletedTask;
        }

        public override ControlScheme[] ControlSchemes => Array.Empty<ControlScheme>();

        public override string Description => _description;

        public override GameplayMode[] GameplayModes => Array.Empty<GameplayMode>();

        public override Genre[] Genres => Array.Empty<Genre>();

        public override Image Image => new Image
        {
            Url = _imageUrl,
            Width = 460,
            Height = 259
        };

        public override DateTime? LastPlayed => null;

        public override LaunchCommand LaunchCommand => GetLaunchCommand();

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override Platform[] Platforms => new[] { WellKnownPlatforms.Windows };

        public override TimeSpan? Playtime => null;

        public override Publisher? Publisher => new Publisher { Name = _publisherName };

        public override DateTime? ReleaseDate => _releaseDate;

        public override Review[] Reviews => Array.Empty<Review>();

        public override string ProviderName => "Epic Games Store";

        public override Studio? Studio => GetStudio();

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _title;

        #region Private methods

        private LaunchCommand GetLaunchCommand()
        {
            bool isLaunchable = !Guid.TryParse(_namespace, out var _);            

            if (isLaunchable)
            {
                var textInfo = CultureInfo.InvariantCulture.TextInfo;
                var nameSpace = textInfo.ToTitleCase(_namespace);

                return new LaunchCommand
                {
                    Icon = "Controller",
                    NewTab = false,
                    Text = "Launch via Epic Games Launcher",
                    Url = $"com.epicgames.launcher://apps/{nameSpace}?action=launch&silent=true"
                };
            }

            var locale = CultureInfo.CurrentCulture.Name.ToLower();

            return new LaunchCommand
            {
                Icon = "Bag",
                NewTab = true,
                Text = "Open in Epic Games Store",
                Url = $"https://store.epicgames.com/{locale}/p/{_urlSlug}"
            };
        }

        private Studio? GetStudio()
        {
            return !string.IsNullOrEmpty(_studioName)
                ? new Studio { Name = _studioName } 
                : null;
        }

        #endregion
    }
}