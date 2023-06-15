using EpicGamesStoreNET.Models;
using GameTracker.Plugins.EpicGames.Interfaces;

namespace GameTracker.Plugins.EpicGames.Wrappers
{
    internal class EpicGamesStoreWrapper : IEpicGamesStore
    {
        public async Task<Response> SearchAsync(string query)
        {
            return await EpicGamesStoreNET.Query.SearchAsync(query);
        }
    }
}