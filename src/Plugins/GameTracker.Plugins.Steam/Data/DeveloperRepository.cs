using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Interfaces.Data;
using GameTracker.Plugins.Steam.Models.Database;

namespace GameTracker.Plugins.Steam.Data
{
    public class DeveloperRepository : DapperRepository<Developer>, IDeveloperRepository
    {
        private const string TableName = "developers";
        private const string BootstrapSql = @"CREATE TABLE developers (
                                                id INTEGER PRIMARY KEY,
                                                name VARCHAR(1000) NOT NULL
                                              );";

        private readonly IDeveloperMappingRepository _developerMappingRepository;

        public DeveloperRepository(IDeveloperMappingRepository developerMappingRepository) 
            : base(Constants.SQLite.ConnectionString, TableName, BootstrapSql)
        {
            _developerMappingRepository = developerMappingRepository;
        }

        public async Task<string[]> GetDevelopers(int appId) 
        {
            var developerMappings = await _developerMappingRepository.GetDeveloperMappings(appId);
            var developerIds = developerMappings.Select(x => x.DeveloperId).Distinct();

            var sql = "SELECT name FROM developers WHERE id IN @developerIds";
            
            return (await GetValue(sql, new { developerIds }))
                .Select(d => d.Name)
                .ToArray();
        }

        public async Task SetDevelopers(int appId, string[] developers)
        {
            const string existsSql = @"SELECT *
                                       FROM developers
                                       WHERE name = @dev
                                       LIMIT 1";

            const string insertSql = @"INSERT INTO developers (id, name)
                                       VALUES (NULL, @dev)";

            foreach (var dev in developers)
            {
                var existing = (await GetValue(existsSql, new { dev })).FirstOrDefault();                
                if (existing == null)
                {
                    await SetValue(insertSql, new { dev });
                    existing = (await GetValue(existsSql, new { dev })).FirstOrDefault();
                }

                if (existing == null)
                {
                    throw new ApplicationException("Failed to update developer table.");
                }

                await _developerMappingRepository.InsertDeveloperMapping(new DeveloperMapping { AppId = appId, DeveloperId = existing.Id });
            }
        }
    }
}