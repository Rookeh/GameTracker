﻿using GameTracker.Interfaces;
using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.GOG.Helpers;
using GameTracker.Plugins.GOG.Models;
using GameTracker.Plugins.GOG.Models.GOGApi;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GameTracker.Plugins.GOG
{
    public class GOGGameProvider : IGameProvider
    {
        private readonly List<GOGGame> _games;
        private readonly Platform _platform;

        private bool _initialized;

        public GOGGameProvider()
        {
            _games = new List<GOGGame>();
            _platform = new Platform
            {
                Name = "GOG",
                Description = "GOG.com is a digital distribution platform for video games and films. It is operated by GOG sp. z o.o., a wholly owned subsidiary of CD Projekt based in Warsaw, Poland. GOG.com delivers DRM-free video games through its digital platform for Microsoft Windows, macOS and Linux.",
                ExtendedInformation = $"This integration requires an authentication code which you can retrieve by visiting " +
                                     @$"<a href=""{Constants.Authentication.AuthenticationCodeUrl}"">this GOG URL</a>, pressing F12 to open your browser dev console, " +
                                       "logging in to the page with your GOG details, and then capturing the <em>code</em> parameter returned in the response header from the " +
                                     @$"'Network' tab of the dev console.<br><br> For more details, see <a href=""{Constants.Authentication.ApiDocumentation}"">here.</a><br><br>" +
                                       "Note: Because the authentication code is short-lived, it is not persisted between sessions. Hence, you will need to manually refresh this integration for each session.",
                Links = new[]
                {
                    new SocialLink
                    {
                        LinkPlatform = LinkType.Web,
                        LinkTarget = "https://www.gog.com/"
                    },
                    new SocialLink
                    {
                        LinkPlatform = LinkType.Twitter,
                        LinkTarget = "@gogcom"
                    },
                    new SocialLink
                    {
                        LinkPlatform = LinkType.YouTube,
                        LinkTarget = "@GOG"
                    }
                }
            };
        }

        public Guid ProviderId => new Guid("8301D3D9-C248-4252-82C4-331CCC7A25E9");

        public Platform Platform => _platform;

        public IEnumerable<Game> Games => _games;

        public Dictionary<string, Type> RequiredParameters => new()
        {
            { "GOG Auth Code", typeof(string) }
        };

        public bool Initialized => _initialized;

        public async Task Load(ParameterCache parameterCache)
        {
            await Refresh(parameterCache.UserId, parameterCache.Parameters);
        }

        public async Task<ParameterCache> Refresh(string userId, params object[] providerSpecificParameters)
        {
            /*
             * 
             * https://gogapidocs.readthedocs.io/en/latest/auth.html
             * https://gogapidocs.readthedocs.io/en/latest/galaxy.html#api-gog-com
             * 
             * Thank you, CDPR. Valve, take notes...
             *
             */

            if (providerSpecificParameters.Length != 1 || !(providerSpecificParameters[0] is string))
            {
                throw new ArgumentException("GOG Auth Code must be provided.");
            }            

            var gogAuthCode = providerSpecificParameters[0] as string;
            var gogAuthToken = await AuthenticationHelper.ExchangeGogAuthCodeForToken(gogAuthCode);
            var authHeader = new AuthenticationHeaderValue("Bearer", gogAuthToken.AccessToken);

            using HttpClient httpClient = new();
            var ownedGamesRequest = new HttpRequestMessage(HttpMethod.Get, "https://embed.gog.com/user/data/games");
            ownedGamesRequest.Headers.Authorization = authHeader;

            var ownedGamesResponse = await httpClient.SendAsync(ownedGamesRequest);            
            if (!ownedGamesResponse.IsSuccessStatusCode)
            {
                ThrowOnApiFailure(ownedGamesResponse);
            }

            var ownedGames = JsonSerializer.Deserialize<OwnedGames>(await ownedGamesResponse.Content.ReadAsStringAsync());
            var ownedGameChunks = ownedGames.Owned.Chunk(50);

            _games.Clear();

            foreach (var chunk in ownedGameChunks)
            {
                var gameDetailsRequestUrl = $"https://api.gog.com/products?ids={string.Join(',', chunk)}?expand=downloads,description,changelog";
                var gameDetailsRequest = new HttpRequestMessage(HttpMethod.Get, gameDetailsRequestUrl);
                var gameDetailsResponse = await httpClient.SendAsync(gameDetailsRequest);

                if (!gameDetailsResponse.IsSuccessStatusCode)
                {
                    ThrowOnApiFailure(gameDetailsResponse);
                }

                var gameDetailsJson = await gameDetailsResponse.Content.ReadAsStringAsync();
                var gameDetails = JsonSerializer.Deserialize<GameDetails[]>(gameDetailsJson);

                _games.AddRange(gameDetails.Select(gd => new GOGGame(gd)));
            }

            _initialized = true;

            // The GOG token is not long-lived, so there is no point in caching it.
            return new ParameterCache
            {
                Parameters = Array.Empty<object>(),
                ProviderId = ProviderId,
                UserId = userId
            };
        }

        private static void ThrowOnApiFailure(HttpResponseMessage response)
        {
            string error = response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                ? "Token was rejected."
                : $"GOG API returned status code {response.StatusCode}.";

            throw new ApplicationException(error);
        }
    }
}