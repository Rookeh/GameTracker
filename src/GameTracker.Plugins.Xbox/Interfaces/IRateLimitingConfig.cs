namespace GameTracker.Plugins.Xbox.Interfaces
{
    public interface IRateLimitingConfig
    {
        int BackOffHours { get; }
        int MaxRequests { get; }
    }
}