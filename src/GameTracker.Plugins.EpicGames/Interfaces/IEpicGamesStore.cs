using EpicGamesStoreNET.Models;

namespace GameTracker.Plugins.EpicGames.Interfaces
{
    public interface IEpicGamesStore
    {
        Task<Response> SearchAsync(string query);
    }
}