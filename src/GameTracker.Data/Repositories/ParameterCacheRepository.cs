using GameTracker.Interfaces.Data;
using GameTracker.Models;

namespace GameTracker.Data.Repositories
{
    public class ParameterCacheRepository : DapperRepository<ParameterCache>, IParameterCacheRepository
    {
        private const string TableName = "parameter_cache";
        private const string CreateSql = @"CREATE TABLE parameter_cache (
                                                userId VARCHAR(1000) NOT NULL,                                                
                                                providerId VARCHAR(36) NOT NULL,
                                                paramOrder INTEGER NOT NULL,
                                                value VARCHAR(1000) NOT NULL
                                              );";
        public ParameterCacheRepository()
            : base(Constants.ConnectionString, TableName, CreateSql)
        {
        }

        public async Task<ParameterCache> GetParameters(string userId, Guid providerId)
        {
            var sql = "SELECT paramOrder, value FROM parameter_cache WHERE userId = @userId AND providerId = @providerId ORDER BY paramOrder";
            IEnumerable<dynamic> results = await GetValueAnonymous(sql, new { userId, providerId = providerId.ToString() });

            var outParams = new List<object>();

            foreach (dynamic result in results)
            {
                outParams.Add(result.value);
            }

            return new ParameterCache
            {
                UserId = userId,
                ProviderId = providerId,
                Parameters = outParams.ToArray()
            };
        }

        public async Task UpdateParameters(ParameterCache value)
        {
            var sql = @"UPDATE parameter_cache 
                        SET value = @value
                        WHERE paramOrder = @order
                        AND providerId = @providerId
                        AND userId = @userId";

            for (int i = 0; i < value.Parameters.Length; i++)
            {
                await SetValue(sql, new
                {
                    userId = value.UserId,
                    providerId = value.ProviderId.ToString(),
                    order = i,
                    value = value.Parameters[i].ToString()
                });
            }
        }

        public async Task InsertParameters(ParameterCache value)
        {
            var sql = @"INSERT INTO parameter_cache (userId, providerId, paramOrder, value)
                        VALUES (@userId, @providerId, @order, @value)";

            for (int i = 0; i < value.Parameters.Length; i++)
            {
                await SetValue(sql, new
                {
                    userId = value.UserId,
                    providerId = value.ProviderId.ToString(),
                    order = i,
                    value = value.Parameters[i].ToString()
                });
            }
        }


    }
}