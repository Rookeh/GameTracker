using Dapper;
using Microsoft.Data.Sqlite;

namespace GameTracker.Plugins.Steam.Data
{
    public abstract class DapperRepository<T>
    {
        private readonly string _connectionString;

        public DapperRepository(string connectionString, string tableName, string createSql)
        {
            _connectionString = connectionString;
            Bootstrap(connectionString, tableName, createSql);
        }

        protected void Bootstrap(string connectionString, string tableName, string bootstrapSql)
        {
            using var connection = new SqliteConnection(connectionString);

            var table = connection.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = @tableName;", new { tableName });
            var tableResult = table.FirstOrDefault();

            if (!string.IsNullOrEmpty(tableResult) && tableResult == tableName)
            {
                return;
            }

            connection.Execute(bootstrapSql);
        }

        protected async Task<IEnumerable<T>> GetValue(string query, object parameters = null)
        {
            using var connection = new SqliteConnection(_connectionString);
            return await connection.QueryAsync<T>(query, parameters);
        }

        protected async Task SetValue(string query, object parameters = null)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.ExecuteAsync(query, parameters);
        }
    }
}