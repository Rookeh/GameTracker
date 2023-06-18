using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Interfaces.Data;
using GameTracker.Plugins.Steam.Models.Database;
using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Data
{
    public class CategoryRepository : DapperRepository<Category>, ICategoryRepository
    {
        private const string TableName = "categories";
        private const string BootstrapSql = @"CREATE TABLE categories (
                                                id INTEGER NOT NULL UNIQUE,
                                                description VARCHAR(1000) NULL
                                              );";

        private readonly ICategoryMappingRepository _categoryMappingRepository;

        public CategoryRepository(ICategoryMappingRepository categoryMappingRepository) 
            : base(Constants.SQLite.ConnectionString, TableName, BootstrapSql)
        {
            _categoryMappingRepository = categoryMappingRepository;
        }

        public async Task<Category[]> GetCategories(int appId)
        {
            var categoryMappings = await _categoryMappingRepository.GetCategoryMappings(appId);
            var categoryIds = categoryMappings.Select(m => m.CategoryId).Distinct();

            const string sql = @"SELECT id, description
                                 FROM categories
                                 WHERE id IN @categoryIds";

            return (await GetValue(sql, new { categoryIds })).ToArray();
        }

        public async Task SetCategories(int appId, Category[] categories)
        {
            const string insertCategory = @"INSERT INTO categories (id, description)
                                            VALUES (@id, @description)
                                            ON CONFLICT DO NOTHING";            

            foreach (var category in categories)
            {
                await SetValue(insertCategory, new { id = category.Id, description = category.Description });
                await _categoryMappingRepository.InsertCategoryMapping(new CategoryMapping { AppId = appId, CategoryId = category.Id });
            }
        }
    }
}