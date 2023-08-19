using GameTracker.Interfaces;
using GameTracker.Models;
using GameTracker.Plugins.Xbox.Helpers;
using GameTracker.Plugins.Xbox.Interfaces;
using GameTracker.Plugins.Xbox.Models;
using System.Net.Http.Headers;

namespace GameTracker.Plugins.Xbox
{
    public class XboxGameProvider : IGameProvider
    {
        private readonly IRateLimitedXboxHttpClient _rateLimitedHttpClient;

        private readonly List<XboxGame> _games;

        private bool _initialized;

        public XboxGameProvider(IRateLimitedXboxHttpClient rateLimitedXboxHttpClient)
        {
            _rateLimitedHttpClient = rateLimitedXboxHttpClient;
            _games = new List<XboxGame>();
        }

        public Guid ProviderId => new Guid("0E2E958C-8705-4C9E-87AC-92874CA05B30");

        public DataPlatform Platform => Constants.XboxPlatform;

        public IEnumerable<Game> Games => _games;

        public Dictionary<string, Type> RequiredParameters => new()
        {
            { "OpenXBL API Key", typeof(string) },
            { "Include Game Pass Titles", typeof(bool) },
            { "Include Legacy Titles", typeof(bool) }
        };

        public bool Initialized => _initialized;

        public async Task Load(ParameterCache parameterCache)
        {
            await Refresh(parameterCache.UserId, parameterCache.Parameters);
        }

        public async Task<ParameterCache> Refresh(string userId, params object[] providerSpecificParameters)
        {
            // https://xbl.io/console
            // n.b. Personal API keys are rate-limited to 150 requests per hour.

            if (providerSpecificParameters.Length < 1 || !(providerSpecificParameters[0] is string))
            {
                throw new ArgumentException("OpenXBL API key must be provided.");
            }

            if (providerSpecificParameters.Length < 2 || !(providerSpecificParameters[1] is bool))
            {
                throw new ArgumentException("Include Game Pass Titles must be boolean.");
            }

            if (providerSpecificParameters.Length < 3 || !(providerSpecificParameters[2] is bool))
            {
                throw new ArgumentException("Include Legacy Titles must be boolean.");
            }

            var apiKey = providerSpecificParameters[0].ToString();
            var includeGamePassTitles = (bool)providerSpecificParameters[1];
            var includeLegacyTitles = (bool)providerSpecificParameters[2];

            var authHeader = new NameValueHeaderValue(Constants.OpenXBL.AuthHeader, apiKey);
            var response = await _rateLimitedHttpClient.GetFromJson(Constants.OpenXBL.TitleHistoryUrl, Constants.DefaultTitleResponse, new[] { authHeader });
            var titlesToInclude = response.Titles;

            if (!includeGamePassTitles)
            {
                titlesToInclude = titlesToInclude.Where(g => !g.GamePass.IsGamePass).ToArray();
            }

            if (!includeLegacyTitles)
            {
                var legacyTitles = titlesToInclude.Where(g => g.Devices.Contains(Constants.Devices.Win32) || g.Devices.Contains(Constants.Devices.Xbox360));
                titlesToInclude = titlesToInclude.Where(x => !legacyTitles.Contains(x)).ToArray();
            }

            _games.Clear();
            _games.AddRange(titlesToInclude.Select(x => new XboxGame(x)));

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