using GameTracker.Plugins.PlayStation.Models.Authentication;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GameTracker.Plugins.PlayStation.Helpers
{
    internal static class AuthenticationHelper
    {
        internal static async Task<string> ExchangeNpssoForCode(string npsso)
        {
            var codeRequestParams = new Dictionary<string, string>()
            {
                ["access_type"] = "offline",
                ["client_id"] = Constants.Authentication.ClientId,
                ["redirect_uri"] = Constants.Authentication.RedirectUri,
                ["response_type"] = "code",
                ["scope"] = Constants.Authentication.Scope
            };

            var requestUri = UriHelper.BuildQueryString($"{Constants.Authentication.AuthenticationBaseUrl}/authorize?", codeRequestParams);
            var codeRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
            codeRequest.Headers.Add("Cookie", $"npsso={npsso}");

            using HttpClient httpClient = new();
            var codeResponse = await httpClient.SendAsync(codeRequest);

            if (codeResponse.StatusCode != HttpStatusCode.Redirect)
            {
                throw new ApplicationException("Unexpected response from PSN API.");
            }

            var locationHeader = codeResponse.Headers.Location?.AbsoluteUri.ToString();
            var locationHeaderParameters = UriHelper.ExtractQueryString(locationHeader);

            if (!locationHeaderParameters.Any() || !locationHeaderParameters.ContainsKey("code"))
            {
                throw new ApplicationException("Failed to obtain PSN token exchange access code. Check that your NPSSO code is correct.");
            }

            return locationHeaderParameters["code"];
        }

        internal static async Task<string> ExchangeCodeForToken(string code)
        {
            using HttpClient httpClient = new();            

            var tokenExchangeParams = new Dictionary<string, string>()
            {
                ["code"] = code,
                ["grant_type"] = "authorization_code",
                ["redirect_uri"] = Constants.Authentication.RedirectUri,
                ["token_format"] = "jwt"
            };

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, $"{Constants.Authentication.AuthenticationBaseUrl}/token");
            tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", Constants.Authentication.TokenExchangeCredential);
            tokenRequest.Content = new FormUrlEncodedContent(tokenExchangeParams);

            var response = await httpClient.SendAsync(tokenRequest);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = new[] { HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized }.Contains(response.StatusCode)
                    ? "PSN NPSSO token exchange was rejected."
                    : $"PSN API returned status code {response.StatusCode}.";

                throw new ApplicationException(errorMessage);
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var tokenExchangeResponse = JsonSerializer.Deserialize<TokenExchangeResponse>(responseJson);

            return tokenExchangeResponse.AccessToken;
        }
    }
}