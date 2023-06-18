using System.Net.Http.Headers;
using System.Text.Json;

namespace GameTracker.Plugins.Common.RateLimiting
{
    public class RateLimitedHttpClient<T>
    {
        private readonly CircuitBreaker<T> _circuitBreaker;
        private readonly List<DateTime> _operations;
        
        private readonly int _backOff;
        private readonly int _maxRequests;

        public RateLimitedHttpClient(TimeSpan backOff, int maxRequests)
        {
            _backOff = Convert.ToInt32(backOff.TotalMinutes * -1);
            _circuitBreaker = new CircuitBreaker<T>(backOff);
            _maxRequests = maxRequests;
            _operations = new List<DateTime>();
        }

        public async Task<T> GetFromJson(string url, T defaultValue, NameValueHeaderValue[] headers = null)
        {
            if (headers == null)
            {
                headers = Array.Empty<NameValueHeaderValue>();
            }

            return await _circuitBreaker.AttemptOperation(() => GetFromJsonImpl(url, defaultValue, headers), ShouldBackoff, defaultValue)
                .ConfigureAwait(false);
        }

        #region Private methods

        private bool ShouldBackoff()
        {
            _operations.RemoveAll(o => o < DateTime.Now.AddMinutes(_backOff));
            return _operations.Count() > _maxRequests;
        }

        private async Task<T> GetFromJsonImpl(string url, T defaultValue, NameValueHeaderValue[] headers)
        {
            try
            {
                using HttpClient client = new();

                foreach (NameValueHeaderValue header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Name, header.Value);
                }

                var response = await client.GetAsync(url).ConfigureAwait(false);
                var contentJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var contentObject = JsonSerializer.Deserialize<T>(contentJson);
                _operations.Add(DateTime.Now);
                return contentObject ?? defaultValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        #endregion
    }
}