using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Interfaces.Data;
using GameTracker.Plugins.Steam.Models.Database;

namespace GameTracker.Plugins.Steam.Data
{
    public class CategoryMappingRepository : DapperRepository<CategoryMapping>, ICategoryMappingRepository
    {
        private const string TableName = "category_mapping";
        private const string BootstrapSql = @"CREATE TABLE category_mapping (
                                                appId INTEGER NOT NULL,
                                                categoryId INTEGER NOT NULL,
                                                UNIQUE(appId, categoryId)
                                              );";

        public CategoryMappingRepository() 
            : base(Constants.SQLite.ConnectionString, TableName, BootstrapSql)
        {
        }

        public async Task<IEnumerable<CategoryMapping>> GetCategoryMappings(int appId)
        {
            const string sql = @"SELECT * FROM category_mapping
                                 WHERE appId = @appId";

            return await GetValue(sql, new { appId });
        }

        public async Task InsertCategoryMapping(CategoryMapping mapping)
        {
            const string sql = @"INSERT INTO category_mapping (appId, categoryId)
                                 VALUES (@appId, @categoryId)
                                 ON CONFLICT DO NOTHING";

            await SetValue(sql, new { appId = mapping.AppId, categoryId = mapping.CategoryId });
        }
    }
}