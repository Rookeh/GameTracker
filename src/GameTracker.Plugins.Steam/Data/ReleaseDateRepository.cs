using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Data
{
    public class ReleaseDateRepository : DapperRepository<ReleaseDate>
    {
        private const string TableName = "release_date";
        private const string BootstrapSql = @"CREATE TABLE release_date (
                                                appId INTEGER NOT NULL,
                                                unreleased INTEGER NOT NULL,
                                                date TEXT NOT NULL
                                              );";

        public ReleaseDateRepository() 
            : base(Constants.SteamGameDetails.ConnectionString, TableName, BootstrapSql)
        {
        }

        public async Task<ReleaseDate?> GetReleaseDate(int appId)
        {
            var sql = @"SELECT unreleased, date
                        FROM release_date
                        WHERE appId = @appId";

            return (await GetValue(sql, new { appId })).FirstOrDefault();
        }

        public async Task SetReleaseDate(int appId, ReleaseDate releaseDate)
        {
            var sql = @"INSERT INTO release_date
                        VALUES (@appId, @unreleased, @date)";

            await SetValue(sql, new { appId, unreleased = releaseDate.Unreleased, date = releaseDate.Date });
        }
    }
}