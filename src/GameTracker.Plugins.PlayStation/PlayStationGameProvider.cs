using GameTracker.Interfaces;
using GameTracker.Models;
using GameTracker.Plugins.Common.Helpers;
using GameTracker.Plugins.PlayStation.Helpers;
using GameTracker.Plugins.PlayStation.Models;
using GameTracker.Plugins.PlayStation.Models.GraphQL;
using System.Net.Http.Headers;
using System.Text.Json;
using Game = GameTracker.Models.Game;

namespace GameTracker.Plugins.PlayStation
{
    public class PlayStationGameProvider : IGameProvider
    {
        private readonly Platform _platform;
        private readonly List<PlayStationGame> _games;

        public PlayStationGameProvider()
        {
            _games = new List<PlayStationGame>();
            _platform = new Platform()
            {
                Name = "PlayStation Network",
                Description = "PlayStation Network",
            };
        }

        public Guid ProviderId => new Guid("A33F4D1B-2B4E-4669-AA98-9207A4F38037");

        public Platform Platform => _platform;

        public IEnumerable<Game> Games => _games;

        public Dictionary<string, Type> RequiredParameters => new()
        {
            { "PSN NPSSO Code", typeof(string) },
            { "Include Non-Game Titles", typeof(bool) }
        };

        public async Task Refresh(params object[] providerSpecificParameters)
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

            var authCode = await AuthenticationHelper.ExchangeNpssoForCode(npsso);
            var authToken = await AuthenticationHelper.ExchangeCodeForToken(authCode);
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

            using HttpClient httpClient = new();
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
        }
    }
}