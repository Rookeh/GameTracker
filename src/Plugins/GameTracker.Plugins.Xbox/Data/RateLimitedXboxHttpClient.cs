using GameTracker.Plugins.Common.RateLimiting;
using GameTracker.Plugins.Xbox.Interfaces;
using GameTracker.Plugins.Xbox.Models.OpenXBL;

namespace GameTracker.Plugins.Xbox.Data
{
    public class RateLimitedXboxHttpClient : RateLimitedHttpClient<XboxLiveTitleResponse>, IRateLimitedXboxHttpClient
    {
        public RateLimitedXboxHttpClient(IRateLimitingConfig rateLimitingConfig) 
            : base(TimeSpan.FromHours(rateLimitingConfig.BackOffHours), rateLimitingConfig.MaxRequests)
        {
        }
    }
}