using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Interfaces.Data;
using GameTracker.Plugins.Steam.Models.Database;

namespace GameTracker.Plugins.Steam.Data
{
    public class GenreMappingRepository : DapperRepository<GenreMapping>, IGenreMappingRepository
    {
        private const string TableName = "genre_mapping";
        private const string BootstrapSql = @"CREATE TABLE genre_mapping (
                                                appId INTEGER NOT NULL,
                                                genreId INTEGER NOT NULL,
                                                UNIQUE(appId, genreId)
                                              );";

        public GenreMappingRepository() 
            : base(Constants.SQLite.ConnectionString, TableName, BootstrapSql)
        {
        }

        public async Task<IEnumerable<GenreMapping>> GetGenreMappings(int appId)
        {
            const string sql = @"SELECT *
                                 FROM genre_mapping
                                 WHERE appId = @appId";

            return await GetValue(sql, new { appId });
        }

        public async Task SetGenreMapping(GenreMapping genreMapping)
        {
            const string sql = @"INSERT INTO genre_mapping (appId, genreId)
                                 VALUES (@appId, @genreId)
                                 ON CONFLICT DO NOTHING";

            await SetValue(sql, new { appId = genreMapping.AppId, genreId = genreMapping.GenreId });
        }
    }
}
