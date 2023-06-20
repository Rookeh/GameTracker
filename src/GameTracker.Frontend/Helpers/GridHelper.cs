using GameTracker.Frontend.Models;
using GameTracker.Models;

namespace GameTracker.Frontend.Helpers
{
    public static class GridHelper
    {
        public static IEnumerable<GameRow> ToGameRows(this IEnumerable<IGrouping<string, Game>> games)
        {
            return games.Chunk(3).Select(c => new GameRow
            {
                Game1 = c[0],
                Game2 = c.Length >= 2 ? c[1] : null,
                Game3 = c.Length >= 3 ? c[2] : null
            });
        }
    }
}