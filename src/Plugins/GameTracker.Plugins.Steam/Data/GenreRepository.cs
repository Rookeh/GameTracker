using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Interfaces.Data;
using GameTracker.Plugins.Steam.Models.Database;
using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Data
{
    public class GenreRepository : DapperRepository<Genre>, IGenreRepository
    {
        private const string TableName = "genres";
        private const string BootstrapSql = @"CREATE TABLE genres (
                                                id INTEGER NOT NULL UNIQUE,
                                                description VARCHAR(1000) NOT NULL
                                              );";

        private readonly IGenreMappingRepository _genreMappingRepository;

        public GenreRepository(IGenreMappingRepository genreMappingRepository)
            : base(Constants.SQLite.ConnectionString, TableName, BootstrapSql)
        {
            _genreMappingRepository = genreMappingRepository;
        }

        public async Task<Genre[]> GetGenres(int appId)
        {
            var genreMappings = await _genreMappingRepository.GetGenreMappings(appId);
            var genreIds = genreMappings.Select(g => g.GenreId).Distinct();

            var sql = @"SELECT id, description
                        FROM genres
                        WHERE id IN @genreIds";

            return (await GetValue(sql, new { genreIds })).ToArray();
        }

        public async Task SetGenres(int appId, Genre[] genres)
        {
            var sql = @"INSERT INTO genres (id, description)
                        VALUES (@id, @description)
                        ON CONFLICT DO NOTHING";

            foreach (var genre in genres)
            {
                await SetValue(sql, new { id = genre.Id, description = genre.Description });
                await _genreMappingRepository.SetGenreMapping(new GenreMapping { AppId = appId, GenreId = genre.Id });
            }
        }
    }
}