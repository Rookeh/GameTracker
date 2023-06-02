using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.Xbox.Models.OpenXBL;

namespace GameTracker.Plugins.Xbox.Models
{
    public class XboxGame : Game
    {
        private readonly Title _xboxTitle;

        public XboxGame(Title xboxTitle)
        {
            PlatformId = Convert.ToInt32(xboxTitle.TitleId);
            _xboxTitle = xboxTitle;
        }

        public override TimeSpan Playtime => TimeSpan.Zero;

        public override string Title => _xboxTitle.Name;

        public override string Description => string.Empty;

        public override Genre[] Genres => Array.Empty<Genre>();

        public override string Image => _xboxTitle.DisplayImage;

        public override string LaunchCommand => $"ms-windows-store://pdp/?PFN={_xboxTitle.PFN}";

        public override MultiplayerAvailability[] MultiplayerAvailability => Array.Empty<MultiplayerAvailability>();

        public override MultiplayerMode[] MultiplayerModes => Array.Empty<MultiplayerMode>();

        public override Platform[] Platforms => _xboxTitle.Devices.Select(d => new Platform { Name = d }).ToArray();

        public override Publisher Publisher => GetPublisher(_xboxTitle.PFN);

        public override DateTime ReleaseDate => DateTime.MinValue;

        public override Review[] Reviews => Array.Empty<Review>();

        public override Studio Studio => new Studio { Name = "Unknown" };

        public override string[] Tags => Array.Empty<string>();

        #region Private methods

        private Publisher GetPublisher(string pfn)
        {
            var pfnArr = pfn.Split('.');

            return new Publisher
            {
                Name = pfnArr.Any() ? pfnArr[0] : "Unknown"
            };
        }

        #endregion
    }
}