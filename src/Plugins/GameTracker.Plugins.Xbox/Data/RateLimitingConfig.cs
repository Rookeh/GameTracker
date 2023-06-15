using GameTracker.Plugins.Xbox.Interfaces;

namespace GameTracker.Plugins.Xbox.Data
{
    public class RateLimitingConfig : IRateLimitingConfig
    {
        public int BackOffHours => 1;
        public int MaxRequests => 150;
    }
}