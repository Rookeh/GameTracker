using IGDB.Models;
using RestEase;

namespace GameTracker.Plugins.IGDB.Interfaces
{
    public interface IIGDBClientWrapper
    {
        Task<CountResponse> CountAsync(string endpoint, string query = null);
        bool IsInvalidTokenResponse(ApiException ex);
        Task<T[]> QueryAsync<T>(string endpoint, string query = null);
    }
}