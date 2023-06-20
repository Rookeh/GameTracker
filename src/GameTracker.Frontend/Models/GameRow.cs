using GameTracker.Models;

namespace GameTracker.Frontend.Models
{
    public class GameRow
    {
        public IGrouping<string, Game>? Game1 { get; set; }
        public IGrouping<string, Game>? Game2 { get; set; }
        public IGrouping<string, Game>? Game3 { get; set; }
    }
}