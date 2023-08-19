using GameTracker.Plugins.IGDB.Interfaces;
using IGDB;
using IGDB.Models;
using RestEase;

namespace GameTracker.Plugins.IGDB.Wrappers
{
    public class IGDBClientWrapper : IIGDBClientWrapper
    {
        private readonly IGDBClient _client;

        public IGDBClientWrapper(IGDBClient client)
        {
            _client = client;
        }

        public async Task<T[]> QueryAsync<T>(string endpoint, string query = null)
        {
            return await _client.QueryAsync<T>(endpoint, query);
        }

        public async Task<CountResponse> CountAsync(string endpoint, string query = null)
        {
            return await _client.CountAsync(endpoint, query);
        }

        public bool IsInvalidTokenResponse(ApiException ex)
        {
            return IGDBClient.IsInvalidTokenResponse(ex);
        }
    }
}