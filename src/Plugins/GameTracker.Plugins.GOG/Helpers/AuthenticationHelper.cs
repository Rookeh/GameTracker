using GameTracker.Plugins.Common.Helpers;
using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.GOG.Interfaces;
using GameTracker.Plugins.GOG.Models.GOGApi;
using System.Text.Json;

namespace GameTracker.Plugins.GOG.Helpers
{
    internal class AuthenticationHelper : IAuthenticationHelper
    {
        private readonly IHttpClientWrapperFactory _httpClientWrapperFactory;

        public AuthenticationHelper(IHttpClientWrapperFactory httpClientWrapperFactory)
        {
            _httpClientWrapperFactory = httpClientWrapperFactory;
        }

        public async Task<AuthToken> ExchangeGogAuthCodeForToken(string code)
        {
            var authParams = new Dictionary<string, string>()
            {
                { "client_id", "46899977096215655" },
                { "client_secret", "9d85c43b1482497dbbce61f6e4aa173a433796eeae2ca8c5f6129f2dc4de46d9" },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", "https://embed.gog.com/on_login_success?origin=client" }
            };

            var requestUri = UriHelper.BuildQueryString($"{Constants.Authentication.AuthenticationBaseUrl}/token?", authParams);

            using var httpClient = _httpClientWrapperFactory.BuildHttpClient();
            var tokenResponse = await httpClient.GetAsync(requestUri);

            if (!tokenResponse.IsSuccessStatusCode)
            {
                throw new ApplicationException("Failed to exchange GOG auth code for token; check the code you provided is correct");
            }

            var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AuthToken>(tokenJson);
        }
    }
}