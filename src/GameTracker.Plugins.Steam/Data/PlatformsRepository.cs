using GameTracker.Plugins.Steam.Helpers;

namespace GameTracker.Plugins.Steam.Data
{
    public class PlatformsRepository : DapperRepository<Platforms>
    {
        private const string TableName = "platforms";
        private const string BootstrapSql = @"CREATE TABLE platforms (
                                                appId INTEGER NOT NULL,
                                                windows INTEGER NOT NULL,
                                                mac INTEGER NOT NULL,
                                                linux INTEGER NOT NULL
                                              );";

        public PlatformsRepository() 
            : base(Constants.SQLite.ConnectionString, TableName, BootstrapSql)
        {
        }

        public async Task<Platforms?> GetPlatforms(int appId)
        {
            var sql = @"SELECT windows, mac, linux
                        FROM platforms
                        WHERE appId = @appId";
            
            var result = await GetValue(sql, new { appId });
            
            return result.FirstOrDefault();
        }

        public async Task SetPlatforms(int appId, Platforms platforms)
        {
            var sql = @"INSERT INTO platforms (appId, windows, mac, linux)
                        VALUES (@appId, @windows, @mac, @linux)";

            await SetValue(sql, new { 
                appId, 
                windows = platforms.Windows,
                mac = platforms.Mac,
                linux = platforms.Linux 
            });
        }
    }
}