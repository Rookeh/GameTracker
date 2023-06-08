using GameTracker.Models;

namespace GameTracker.Interfaces.Data
{
    public interface IParameterCacheRepository
    {
        Task<ParameterCache> GetParameters(string userId, Guid providerId);
        Task SetParameters(ParameterCache value);
    }
}
