using GameTracker.Data.Repositories;
using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Data
{
    public class CategoryRepository : DapperRepository<Category>
    {
        private const string TableName = "categories";
        private const string BootstrapSql = @"CREATE TABLE categories (
                                                appId INTEGER NOT NULL,
                                                id INTEGER NOT NULL,
                                                description VARCHAR(1000) NULL
                                              );";

        public CategoryRepository() 
            : base(Constants.SQLite.ConnectionString, TableName, BootstrapSql)
        {
        }

        public async Task<Category[]> GetCategories(int appId)
        {
            var sql = @"SELECT id, description
                        FROM categories
                        WHERE appId = @appId";

            return (await GetValue(sql, new { appId })).ToArray();
        }

        public async Task SetCategories(int appId, Category[] categories)
        {
            var sql = @"INSERT INTO categories (appId, id, description)
                        VALUES (@appId, @id, @description)";

            foreach (var category in categories)
            {
                await SetValue(sql, new { appId, id = category.Id, description = category.Description });
            }
        }
    }
}