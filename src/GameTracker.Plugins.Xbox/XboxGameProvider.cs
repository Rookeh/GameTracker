﻿using GameTracker.Interfaces;
using GameTracker.Models;
using GameTracker.Plugins.Common.RateLimiting;
using GameTracker.Plugins.Xbox.Helpers;
using GameTracker.Plugins.Xbox.Models;
using GameTracker.Plugins.Xbox.Models.OpenXBL;
using System.Net.Http.Headers;

namespace GameTracker.Plugins.Xbox
{
    public class XboxGameProvider : IGameProvider
    {
        private readonly RateLimitedHttpClient<XboxLiveTitleResponse> _rateLimitedHttpClient;
        private const int BackOffHours = 1;
        private const int MaxRequests = 150;

        private readonly List<XboxGame> _games;

        public XboxGameProvider()
        {
            _rateLimitedHttpClient = new RateLimitedHttpClient<XboxLiveTitleResponse>(TimeSpan.FromHours(BackOffHours), MaxRequests);
            _games = new List<XboxGame>();
        }

        public Guid ProviderId => new Guid("0E2E958C-8705-4C9E-87AC-92874CA05B30");

        public Platform Platform => Constants.XboxPlatform;

        public IEnumerable<Game> Games => _games;

        public Dictionary<string, Type> RequiredParameters => new()
        {
            { "OpenXBL API Key", typeof(string) },
            { "Include Game Pass Titles", typeof(bool) },
            { "Include PC only Titles", typeof(bool) }
        };

        public async Task Refresh(params object[] providerSpecificParameters)
        {
            // https://xbl.io/console
            // n.b. Personal API keys are rate-limited to 150 requests per hour.

            if (!(providerSpecificParameters[0] is string))
            {
                throw new ArgumentException("OpenXBL API key must be provided.");
            }

            if (!(providerSpecificParameters[1] is bool))
            {
                throw new ArgumentException("Include Game Pass Titles must be boolean.");
            }

            if (!(providerSpecificParameters[2] is bool))
            {
                throw new ArgumentException("Include PC only Titles must be boolean.");
            }

            var apiKey = providerSpecificParameters[0].ToString();
            var includeGamePassTitles = (bool)providerSpecificParameters[1];
            var includePcOnlyTitles = (bool)providerSpecificParameters[2];

            var authHeader = new NameValueHeaderValue(Constants.OpenXBL.AuthHeader, apiKey);
            var response = await _rateLimitedHttpClient.GetFromJson(Constants.OpenXBL.TitleHistoryUrl, Constants.DefaultTitleResponse, new[] { authHeader });
            var xboxTitles = response.Titles;

            if (!includeGamePassTitles)
            {
                xboxTitles = xboxTitles.Where(g => !g.GamePass.IsGamePass).ToArray();
            }

            if (!includePcOnlyTitles)
            {
                var pcOnlyTitles = xboxTitles.Where(g => g.Devices.Length == 1 && new[] { Constants.Devices.PC, Constants.Devices.Win32 }.Contains(g.Devices[0])).ToArray();
                xboxTitles = xboxTitles.Where(x => !pcOnlyTitles.Contains(x)).ToArray();
            }

            _games.Clear();
            _games.AddRange(xboxTitles.Select(x => new XboxGame(x)));
        }
    }
}