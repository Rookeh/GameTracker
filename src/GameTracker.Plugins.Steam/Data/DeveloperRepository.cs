using GameTracker.Data.Repositories;
using GameTracker.Plugins.Steam.Helpers;

namespace GameTracker.Plugins.Steam.Data
{
    public class DeveloperRepository : DapperRepository<string>
    {
        private const string TableName = "developers";
        private const string BootstrapSql = @"CREATE TABLE developers (
                                                appId INTEGER NOT NULL,
                                                developer VARCHAR(1000) NOT NULL
                                              );";

        public DeveloperRepository() 
            : base(Constants.SQLite.ConnectionString, TableName, BootstrapSql)
        {
        }

        public async Task<string[]> GetDevelopers(int appId) 
        {
            var sql = "SELECT developer FROM developers WHERE appId = @appId";
            return (await GetValue(sql, new { appId })).ToArray();
        }

        public async Task SetDevelopers(int appId, string[] developers)
        {
            foreach(var dev in developers)
            {
                var sql = @"INSERT INTO developers (appId, developer)
                            VALUES (@appId, @dev)";
                
                await SetValue(sql, new { appId, dev });
            }
        }
    }
}