using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Interfaces.Data;
using GameTracker.Plugins.Steam.Models.Database;
using Publisher = GameTracker.Plugins.Steam.Models.Database.Publisher;

namespace GameTracker.Plugins.Steam.Data
{
    public class PublisherRepository : DapperRepository<Publisher>, IPublisherRepository
    {
        private const string TableName = "publishers";
        private const string BootstrapSql = @"CREATE TABLE publishers (
                                                id INTEGER PRIMARY KEY,
                                                name VARCHAR(1000) NOT NULL
                                              );";

        private readonly IPublisherMappingRepository _publisherMappingRepository;

        public PublisherRepository(IPublisherMappingRepository publisherMappingRepository)
            : base(Constants.SQLite.ConnectionString, TableName, BootstrapSql)
        {
            _publisherMappingRepository = publisherMappingRepository;
        }

        public async Task<string[]> GetPublishers(int appId)
        {
            var publisherMappings = await _publisherMappingRepository.GetPublisherMappings(appId);
            var publisherIds = publisherMappings.Select(x => x.PublisherId).Distinct();

            var sql = "SELECT name FROM publishers WHERE id IN @publisherIds";

            return (await GetValue(sql, new { publisherIds }))
                .Select(d => d.Name)
                .ToArray();
        }

        public async Task SetPublishers(int appId, string[] publishers)
        {
            const string existsSql = @"SELECT *
                                       FROM publishers
                                       WHERE name = @pub
                                       LIMIT 1";

            const string insertSql = @"INSERT INTO publishers (id, name)
                                       VALUES (NULL, @pub)"
            ;

            foreach (var pub in publishers)
            {
                var existing = (await GetValue(existsSql, new { pub })).FirstOrDefault();
                if (existing == null)
                {
                    await SetValue(insertSql, new { pub });
                    existing = (await GetValue(existsSql, new { pub })).FirstOrDefault();
                }

                if (existing == null)
                {
                    throw new ApplicationException("Failed to update publisher table.");
                }

                await _publisherMappingRepository.InsertPublisherMapping(new PublisherMapping { AppId = appId, PublisherId = existing.Id });
            }
        }
    }
}
