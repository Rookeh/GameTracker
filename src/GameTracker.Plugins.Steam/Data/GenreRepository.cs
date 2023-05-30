using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Data
{
    public class GenreRepository : DapperRepository<Genre>
    {
        private const string TableName = "genres";
        private const string BootstrapSql = @"CREATE TABLE genres (
                                                appId INTEGER NOT NULL,
                                                id VARCHAR(1000) NOT NULL,
                                                description VARCHAR(1000) NOT NULL
                                              );";

        public GenreRepository() 
            : base(Constants.ConnectionString, TableName, BootstrapSql)
        {
        }

        public async Task<Genre[]> GetGenres(int appId)
        {
            var sql = @"SELECT id, description
                        FROM genres
                        WHERE appId = @appId";

            return (await GetValue(sql, new { appId })).ToArray();
        }

        public async Task SetGenres(int appId, Genre[] genres)
        {
            var sql = @"INSERT INTO genres
                        VALUES (@appId, @id, @description)";

            foreach (var genre in genres) 
            {
                await SetValue(sql, new { appId, id = genre.Id, description = genre.Description });
            }
        }
    }
}