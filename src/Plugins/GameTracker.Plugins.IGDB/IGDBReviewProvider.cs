using GameTracker.Interfaces;
using GameTracker.Models;
using GameTracker.Models.Enums;
using GameTracker.Plugins.IGDB.Helpers;
using GameTracker.Plugins.IGDB.Interfaces;

namespace GameTracker.Plugins.IGDB
{
    public class IGDBReviewProvider : IReviewProvider
    {        
        private readonly IIGDBClientWrapperFactory _igdbClientFactory;

        private bool _initialized;

        public IGDBReviewProvider(IIGDBClientWrapperFactory igdbClientFactory)
        {
            _initialized = false;
            _igdbClientFactory = igdbClientFactory;
        }
        public IEnumerable<Game> Games { get; set; }

        public Guid ProviderId => new Guid("3AFD291F-7538-453C-9D38-7396DA856AAE");

        public bool Initialized => _initialized;

        public DataPlatform Platform => new DataPlatform
        {
            Name = "IGDB.com",
            Icon = "Database",
            Description = "The Internet Game Database (IGDB) is an online database containing information, user reviews and metadata about video games.",
            ExtendedInformation = "This database can be accessed via <a href=\"https://dev.twitch.tv/console\">Twitch Developer</a> authentication; you must provide a client ID and client secret to authenticate with.",
            Links = new[]
            {
                new SocialLink
                {
                    LinkPlatform = LinkType.Web,
                    LinkTarget = "https://www.igdb.com"
                }
            }  
        };

        public Dictionary<string, Type> RequiredParameters => new()
        {
            { "Client ID", typeof(string) },
            { "Client Secret", typeof(string) }
        };

        public async Task Load(ParameterCache parameterCache)
        {
            await Refresh(parameterCache.UserId, parameterCache.Parameters);
        }

        public async Task<ParameterCache> Refresh(string userName, params object[] providerSpecificParameters)
        {
            if (providerSpecificParameters.Length != 2)
            {
                throw new ArgumentException("Incorrect number of arguments.");
            }

            if (!(providerSpecificParameters[0] is string) || string.IsNullOrEmpty(providerSpecificParameters[0].ToString()))
            {
                throw new ArgumentException("IGDB Client ID must be provided.");
            }

            if (!(providerSpecificParameters[1] is string) || string.IsNullOrEmpty(providerSpecificParameters[1].ToString()))
            {
                throw new ArgumentException("IGDB Client Secret must be provided.");
            }

            var clientId = providerSpecificParameters[0].ToString();
            var clientSecret = providerSpecificParameters[1].ToString();
            var igdbClient = _igdbClientFactory.BuildIGDBClientWrapper(clientId, clientSecret);

            var gameChunks = Games.Where(g => RequiresHydration(g)).Chunk(10);

            string query = string.Empty;

            try
            {               
                foreach (var chunk in gameChunks)
                {
                    // https://api-docs.igdb.com/#multi-query
                    query = QueryBuilder.BuildQuery(chunk);                    
                    var results = await igdbClient.QueryAsync<IGDBQueryResult>("multiquery", query);
                    foreach (var result in results)
                    {
                        var matchingResult = result.Game.FirstOrDefault();
                        var matchingGame = Games.FirstOrDefault(g => g.PlatformId.ToString() == result.Id);
                        if (matchingGame == null || matchingResult == null)
                        {
                            continue;
                        }

                        matchingGame.Reviews.Add(new Review
                        {
                            Critic = "IGDB",
                            Score = matchingResult.Rating,
                            UpperBound = 100,
                            Url = matchingResult.Url
                        });

                        if (string.IsNullOrEmpty(matchingGame.Description) && !string.IsNullOrEmpty(matchingResult.Summary))
                        {
                            matchingGame.Description = matchingResult.Summary;
                        }

                        if ((matchingGame.GameplayModes == null || !matchingGame.GameplayModes.Any()) && (matchingResult.GameModes != null && matchingResult.GameModes.Any()))
                        {
                            matchingGame.GameplayModes = matchingResult.GameModes.Select(g => g.ToGameplayMode()).ToArray();
                        }

                        if ((matchingGame.Genres == null || !matchingGame.Genres.Any()) && (matchingResult.Genres != null && matchingResult.Genres.Any()))
                        {
                            matchingGame.Genres = matchingResult.Genres.Select(g => g.ToGenre()).ToArray();
                        }

                        if (matchingGame.ReleaseDate == null && matchingResult.FirstReleaseDate > 0)
                        {
                            matchingGame.ReleaseDate = DateTime.UnixEpoch.AddSeconds(matchingResult.FirstReleaseDate);
                        }
                    }
                }

                _initialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Query: {query}, exception: {ex}");
            }

            return new ParameterCache
            {
                Parameters = providerSpecificParameters,
                ProviderId = ProviderId,
                UserId = userName
            };
        }

        private static bool RequiresHydration(Game g)
        {
            return string.IsNullOrEmpty(g.Description)
                || (g.Reviews == null || !g.Reviews.Any())
                || (g.GameplayModes == null || !g.GameplayModes.Any())
                || (g.Genres == null || !g.Genres.Any())
                || g.ReleaseDate == null;
        }
    }
}