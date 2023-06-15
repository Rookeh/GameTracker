using EpicGamesStoreNET.Models;
using GameTracker.Models;
using GameTracker.Models.Constants;
using GameTracker.Models.Enums;
using System.Globalization;

namespace GameTracker.Plugins.EpicGames.Models
{
    public class EpicGame : Game
    {
        private readonly Element _element;

        internal EpicGame(Element element)
        {
            var hexBytes = Convert.FromHexString(element.Id);
            PlatformId = BitConverter.ToInt32(hexBytes);
            _element = element;
        }

        public override Task Preload()
        {
            return Task.CompletedTask;
        }

        public override ControlScheme[] ControlSchemes => Array.Empty<ControlScheme>();

        public override string Description => _element.Description;

        public override GameplayMode[] GameplayModes => Array.Empty<GameplayMode>();

        public override Genre[] Genres => Array.Empty<Genre>();

        public override Image Image => new Image
        {
            Url = _element.KeyImages?.FirstOrDefault(i => i.Type == "OfferImageWide")?.Url ?? "img//placeholder.png",
            Width = 460,
            Height = 259
        };

        public override DateTime? LastPlayed => null;

        public override LaunchCommand LaunchCommand => GetLaunchCommand();

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override Platform[] Platforms => new[] { WellKnownPlatforms.Windows };

        public override TimeSpan? Playtime => null;

        public override Publisher? Publisher => new Publisher { Name = _element.Seller.Name };

        public override DateTime? ReleaseDate => _element.EffectiveDate;

        public override Review[] Reviews => Array.Empty<Review>();

        public override string StorefrontName => "Epic";

        public override Studio? Studio => GetStudio();

        public override string[] Tags => Array.Empty<string>();

        public override string Title => _element.Title;

        #region Private methods

        private LaunchCommand GetLaunchCommand()
        {
            bool isLaunchable = !Guid.TryParse(_element.Namespace, out var _);            

            if (isLaunchable)
            {
                var textInfo = CultureInfo.InvariantCulture.TextInfo;
                var nameSpace = textInfo.ToTitleCase(_element.Namespace);

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
                Url = $"https://store.epicgames.com/{locale}/p/{_element.UrlSlug}"
            };
        }

        private Studio? GetStudio()
        {
            var developerAttribute = _element.CustomAttributes.FirstOrDefault(ca => ca.Key == "developerName");
            return developerAttribute != null 
                ? new Studio { Name = developerAttribute.Value } 
                : null;
        }

        #endregion
    }
}