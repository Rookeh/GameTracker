using GameTracker.Interfaces;
using GameTracker.Models;
using GameTracker.Plugins.Common.Helpers;
using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.PlayStation.Helpers;
using GameTracker.Plugins.PlayStation.Interfaces;
using GameTracker.Plugins.PlayStation.Models;
using GameTracker.Plugins.PlayStation.Models.GraphQL;
using System.Net.Http.Headers;
using System.Text.Json;
using Game = GameTracker.Models.Game;

namespace GameTracker.Plugins.PlayStation
{
    public class PlayStationGameProvider : IGameProvider
    {
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly IHttpClientWrapperFactory _httpClientWrapperFactory;

        private readonly List<PlayStationGame> _games;
        private bool _initialized;

        public PlayStationGameProvider(IAuthenticationHelper authenticationHelper, IHttpClientWrapperFactory httpClientWrapperFactory)
        {            
            _authenticationHelper = authenticationHelper;
            _httpClientWrapperFactory = httpClientWrapperFactory;
            _games = new List<PlayStationGame>();
        }

        public Guid ProviderId => new Guid("A33F4D1B-2B4E-4669-AA98-9207A4F38037");

        public Platform Platform => new Platform
        {
            Name = "PlayStation Network",
            Description = "The PlayStation Network (PSN) is an online gaming and digital storefront owned and operated by Sony Interactive Entertainment. " +
                          "It was launched in 2006 as the online platform for the PlayStation 3 console, and it has been integrated into every PlayStation " +
                           "console since (with the exception of the PlayStation Classic). It offers the ability to purchase digital titles, and until 2021 it " +
                           "also offered digital media purchases of TV and movies, however this has since been discontinued with the rise of other streaming " +
                           "services. PSN also offers a subscription service, PlayStation Plus, which is a competitor to Microsoft's Xbox Game Pass.",
            ExtendedInformation = "This integration requires your PSN 'NPSSO' token to fetch owned games from your profile. To obtain this, first " +
                                @$"<a href=""{Constants.Authentication.UserLoginLink}"">log into the PlayStation network</a>. Once you are logged " +
                                @$"in, you can retrieve your token from <a href=""{Constants.Authentication.NPSSOTokenLink}"">this</a> link.<br><br> " +
                                  "Note that only your 10 most recently played titles are exposed by the PSN API. These include non-game titles " +
                                  "(e.g. streaming services). These titles can be included or excluded from the results.",
            Icon = "Playstation"
        };

        public IEnumerable<Game> Games => _games;

        public Dictionary<string, Type> RequiredParameters => new()
        {
            { "PSN NPSSO Code", typeof(string) },
            { "Include Non-Game Titles", typeof(bool) }
        };

        public bool Initialized => _initialized;

        public async Task Load(ParameterCache parameterCache)
        {
            await Refresh(parameterCache.UserId, parameterCache.Parameters);
        }

        public async Task<ParameterCache> Refresh(string userId, params object[] providerSpecificParameters)
        {
            if (!(providerSpecificParameters[0] is string))
            {
                throw new ArgumentException("PSN NPSSO Code must be a string.");
            }

            if (!(providerSpecificParameters[1] is bool))
            {
                throw new ArgumentException("Include Non-Game Titles must be a boolean.");
            }

            var npsso = providerSpecificParameters[0] as string;
            var includeNonGameTitles = (bool)providerSpecificParameters[1];

            var authCode = await _authenticationHelper.ExchangeNpssoForCode(npsso);
            var authToken = await _authenticationHelper.ExchangeCodeForToken(authCode);
            var authHeader = new AuthenticationHeaderValue("Bearer", authToken);

            var gameQuery = new PersistedQueryRoot
            {
                PersistedQuery = new PersistedQuery
                {
                    Version = 1,
                    SHA256Hash = Constants.GraphQL.GetUserGameListHash
                }
            };

            var queryParams = new Dictionary<string, string>()
            {
                ["operationName"] = Constants.GraphQL.GetUserGameOperation,
                ["variables"] = Constants.GraphQL.GetUserGameFilter,
                ["extensions"] = JsonSerializer.Serialize(gameQuery)
            };

            Uri gameRequestUri = UriHelper.BuildQueryString(Constants.GraphQL.GraphQLBaseUrl, queryParams);
            var gameRequest = new HttpRequestMessage(HttpMethod.Get, gameRequestUri);
            gameRequest.Headers.Authorization = authHeader;

            using var httpClient = _httpClientWrapperFactory.BuildHttpClient();
            var gameResponse = await httpClient.SendAsync(gameRequest);

            if (!gameResponse.IsSuccessStatusCode)
            {
                throw new ApplicationException($"PSN API call failed with status code {gameResponse.StatusCode}.");
            }

            var gameResponseJson = await gameResponse.Content.ReadAsStringAsync();
            var psnGames = JsonSerializer.Deserialize<GameResponse>(gameResponseJson);

            if (psnGames != null && psnGames.Data.GameLibraryTitlesRetrieve.Games.Any())
            {
                var gameTitles = psnGames.Data.GameLibraryTitlesRetrieve.Games;
                if (!includeNonGameTitles)
                {
                    gameTitles = gameTitles.Where(g => !string.IsNullOrEmpty(g.ProductId)).ToArray();
                }

                _games.Clear();
                _games.AddRange(gameTitles.Select(g => new PlayStationGame(g)));
            }

            _initialized = true;

            return new ParameterCache
            {
                Parameters = providerSpecificParameters,
                ProviderId = ProviderId,
                UserId = userId
            };
        }
    }
}