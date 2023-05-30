using System.Text.Json;

namespace GameTracker.Plugins.Steam.RateLimiting
{
    internal class RateLimitedHttpClient<T>
    {
        private readonly CircuitBreaker<T> _circuitBreaker;
        private readonly List<DateTime> _operations;

        internal RateLimitedHttpClient()
        {
            _circuitBreaker = new CircuitBreaker<T>(TimeSpan.FromMinutes(5));
            _operations = new List<DateTime>();
        }

        internal async Task<T> Get(string url, T defaultValue)
        {
            return await _circuitBreaker.AttemptOperation(() => QueryApi(url, defaultValue), ShouldBackoff, defaultValue)
                .ConfigureAwait(false);
        }

        private bool ShouldBackoff()
        {
            _operations.RemoveAll(o => o < DateTime.Now.AddMinutes(-5));
            return _operations.Count() > 15;
        }

        private async Task<T> QueryApi(string url, T defaultValue)
        {
            using HttpClient client = new();
            var response = await client.GetAsync(url).ConfigureAwait(false);
            var contentJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentObject = JsonSerializer.Deserialize<T>(contentJson);
            _operations.Add(DateTime.Now);
            return contentObject ?? defaultValue;
        }
    }
}