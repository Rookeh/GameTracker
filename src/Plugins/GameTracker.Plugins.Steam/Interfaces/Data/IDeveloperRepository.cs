namespace GameTracker.Plugins.Steam.Interfaces.Data
{
    public interface IDeveloperRepository
    {
        Task<string[]> GetDevelopers(int appId);
        Task SetDevelopers(int appId, string[] developers);
    }
}