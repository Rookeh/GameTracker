using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Interfaces.Data;
using GameTracker.Plugins.Steam.Models.Database;

namespace GameTracker.Plugins.Steam.Data
{
    public class PublisherMappingRepository : DapperRepository<PublisherMapping>, IPublisherMappingRepository
    {
        private const string TableName = "publisher_mapping";
        private const string BootstrapSql = @"CREATE TABLE publisher_mapping (
                                                appId INTEGER NOT NULL,
                                                publisherId INTEGER NOT NULL,
                                                UNIQUE(appId, publisherId)
                                              );";

        public PublisherMappingRepository()
            : base(Constants.SQLite.ConnectionString, TableName, BootstrapSql)
        {
        }

        public async Task<IEnumerable<PublisherMapping>> GetPublisherMappings(int appId)
        {
            const string sql = @"SELECT * FROM publisher_mapping
                                 WHERE appId = @appId";

            return await GetValue(sql, new { appId });
        }

        public async Task InsertPublisherMapping(PublisherMapping mapping)
        {
            const string sql = @"INSERT INTO publisher_mapping (appId, publisherId)
                                 VALUES (@appId, @publisherId)
                                 ON CONFLICT DO NOTHING";

            await SetValue(sql, new { appId = mapping.AppId, publisherId = mapping.PublisherId });
        }
    }
}