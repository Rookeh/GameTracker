using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Interfaces.Data;
using GameTracker.Plugins.Steam.Models.Database;

namespace GameTracker.Plugins.Steam.Data
{
    public class DeveloperMappingRepository : DapperRepository<DeveloperMapping>, IDeveloperMappingRepository
    {
        private const string TableName = "developer_mapping";
        private const string BootstrapSql = @"CREATE TABLE developer_mapping (
                                                appId INTEGER NOT NULL,
                                                developerId INTEGER NOT NULL,
                                                UNIQUE(appId, developerId)
                                              );";

        public DeveloperMappingRepository() 
            : base(Constants.SQLite.ConnectionString, TableName, BootstrapSql)
        {
        }

        public async Task<IEnumerable<DeveloperMapping>> GetDeveloperMappings(int appId)
        {
            const string sql = @"SELECT * FROM developer_mapping
                                 WHERE appId = @appId";

            return await GetValue(sql, new { appId });
        }

        public async Task InsertDeveloperMapping(DeveloperMapping mapping)
        {
            const string sql = @"INSERT INTO developer_mapping (appId, developerId)
                                 VALUES (@appId, @developerId)
                                 ON CONFLICT DO NOTHING";

            await SetValue(sql, new { appId = mapping.AppId, developerId = mapping.DeveloperId });
        }
    }
}