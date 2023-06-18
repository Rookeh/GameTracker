using GameTracker.Plugins.Steam.Models.Database;

namespace GameTracker.Plugins.Steam.Interfaces.Data
{
    public interface IGenreMappingRepository
    {
        Task<IEnumerable<GenreMapping>> GetGenreMappings(int appId);
        Task SetGenreMapping(GenreMapping genreMapping);
    }
}