namespace GameTracker.Plugins.Steam.Interfaces.Data
{
    public interface IPlatformsRepository
    {
        Task<Platforms?> GetPlatforms(int appId);
        Task SetPlatforms(int appId, Platforms platforms);
    }
}