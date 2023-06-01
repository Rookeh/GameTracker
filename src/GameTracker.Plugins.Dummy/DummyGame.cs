using GameTracker.Models;
using GameTracker.Models.Enums;

namespace GameTracker.Plugins.Dummy
{
    public class DummyGame : Game
    {
        private readonly string _title;

        public DummyGame()
        {
            _title = "Dummy Title";
            PlatformId = 0;
        }

        public override string Description => "Dummy Description";

        public override Genre[] Genres => new[] { Genre.Other };

        public override string Image => "https://upload.wikimedia.org/wikipedia/commons/2/21/Hello_World_Brian_Kernighan_1978.jpg";

        public override string LaunchCommand => "https://en.wikipedia.org/wiki/%22Hello,_World!%22_program";

        public override MultiplayerAvailability[] MultiplayerAvailability => new[] { Models.Enums.MultiplayerAvailability.None };

        public override MultiplayerMode[] MultiplayerModes => new[] { MultiplayerMode.None } ;

        public override Platform[] Platforms => new[] { new Platform { Name = "Dummy Platform" } } ;

        public override TimeSpan Playtime => TimeSpan.Zero;

        public override Publisher Publisher => new Publisher { Name = "Dummy Publisher" };

        public override DateTime ReleaseDate => DateTime.Today;

        public override Review[] Reviews => new[] { new Review { Score = new Random().Next(11) } } ;

        public override Studio Studio => new Studio { Name = "Dummy Studio" };

        public override string[] Tags => new[] { "test", "hello", "world" } ;        

        public override string Title => _title;
    }
}