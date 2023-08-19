using GameTracker.Interfaces;
using GameTracker.Models;
using GameTracker.Plugins.Common.Helpers;
using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.Nintendo.Helpers;
using GameTracker.Plugins.Nintendo.Models;
using GameTracker.Plugins.Nintendo.Models.EU;
using GameTracker.Plugins.Nintendo.Models.JP;
using System.Xml.Serialization;

namespace GameTracker.Plugins.Nintendo
{
    public class NintendoGameProvider : IGameProvider
    {
        private readonly IHttpClientWrapperFactory _httpClientWrapperFactory;

        private readonly List<Game> _games;        
        private bool _initialized;

        public NintendoGameProvider(IHttpClientWrapperFactory httpClientWrapperFactory)
        {
            _games = new List<Game>();
            _httpClientWrapperFactory = httpClientWrapperFactory;
        }

        public Guid ProviderId => new Guid("BD638153-9288-4467-84BE-4EF73CE102DD");

        public DataPlatform Platform => Constants.Platforms.NintendoEShop;

        public IEnumerable<Game> Games => _games;

        public Dictionary<string, Type> RequiredParameters => new()
        {
            { "Region code", typeof(string) },
            { "Semicolon-delimited list of titles", typeof(string) }
        };

        public bool Initialized => _initialized;

        public Task Load(ParameterCache parameterCache)
        {
            return Task.CompletedTask;
        }

        public async Task<ParameterCache> Refresh(string userName, params object[] providerSpecificParameters)
        {
            if (providerSpecificParameters[0] == null || !(providerSpecificParameters[0] is string))
            {
                throw new ArgumentException("Region code must be provided.");
            }

            if (providerSpecificParameters[1] == null || !(providerSpecificParameters[1] is string))
            {
                throw new ArgumentException("Semicolon-delimited list of titles must be provided.");
            }

            string regionString = providerSpecificParameters[0] as string;
            if (!Enum.TryParse<Region>(regionString.ToUpper(), out var region))
            {
                throw new ArgumentException("Invalid region code (must be EU, JP or NA).");
            }

            string delimitedTitles = providerSpecificParameters[1] as string;                        
            string[] searchTitles = delimitedTitles.Split(';');

            _games.Clear();

            switch (region)
            {
                case Region.EU:
                    _games.AddRange((await PopulateEUGames(searchTitles)).ToArray());
                    break;
                case Region.JP:
                    _games.AddRange((await PopulateJPGames(searchTitles)).ToArray());
                    break;
                case Region.NA:
                    throw new NotSupportedException("North America region is not currently supported.");
            }

            return new ParameterCache
            {
                Parameters = providerSpecificParameters,
                ProviderId = ProviderId,
                UserId = userName
            };
        }

        private async Task<IEnumerable<EUNintendoGame>> PopulateEUGames(string[] searchTitles)
        {
            var gameQueryDict = new Dictionary<string, string>()
            {
                ["fq"] = "type:GAME AND system_type:nintendoswitch* AND product_code_txt:*",
                ["q"] = "*",
                ["rows"] = Constants.EURegion.MaxGames,
                ["sort"] = "sorting_title asc",
                ["start"] = "0",
                ["wt"] = "json"
            };

            var baseUrl = string.Format(Constants.EURegion.GetGamesUrlFormat, Constants.EURegion.DefaultLocale);
            var requestUri = UriHelper.BuildQueryString(baseUrl, gameQueryDict);
            using var client = _httpClientWrapperFactory.BuildHttpClient();
            var euGameResponseRoot = await client.GetFromJsonAndTypeAsync(requestUri, typeof(EUGameResponseRoot)) as EUGameResponseRoot;

            if (euGameResponseRoot != null)
            {
                return euGameResponseRoot.Response.Docs.Where(d => searchTitles.Any(st => d.Title.Contains(st)))
                    .Select(d => new EUNintendoGame(d));
            }

            return Enumerable.Empty<EUNintendoGame>();
        }

        private async Task<IEnumerable<JPNintendoGame>> PopulateJPGames(string[] searchTitles)
        {
            using var client = _httpClientWrapperFactory.BuildHttpClient();
            var gamesResponse = await client.GetStringAsync(Constants.JPRegion.GetGamesUrl);

            if (!string.IsNullOrEmpty(gamesResponse))
            {
                var serializer = new XmlSerializer(typeof(TitleInfoList));
                TitleInfoList titleInfoList;

                using (TextReader textReader = new StringReader(gamesResponse))
                {
                    titleInfoList = (TitleInfoList)serializer.Deserialize(textReader);
                }

                return titleInfoList.TitleInfo.Where(t => searchTitles.Any(st => t.TitleName.Contains(st)))
                    .Select(t => new JPNintendoGame(t));
            }

            return Enumerable.Empty<JPNintendoGame>();
        }
    }
}