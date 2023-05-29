using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Data
{
    public class MetacriticScoreRepository : DapperRepository<MetacriticScore>
    {
        private const string TableName = "metacritic";
        private const string BootstrapSql = @"CREATE TABLE metacritic (
                                                appId INTEGER NOT NULL,
                                                score REAL NOT NULL,
                                                url VARCHAR(1000) NULL
                                              );";

        public MetacriticScoreRepository() 
            : base(Constants.SteamGameDetails.ConnectionString, TableName, BootstrapSql)
        {
        }

        public async Task<MetacriticScore?> GetMetacriticScore(int appId)
        {
            var sql = @"SELECT score, url
                        FROM metacritic
                        WHERE appId = @appId";

            return (await GetValue(sql, new { appId })).FirstOrDefault();
        }

        public async Task SetMetacriticScore(int appId, MetacriticScore metacriticScore)
        {
            var sql = @"INSERT INTO metacritic (appId, score, url)
                        VALUES (@appId, @score, @url)";

            await SetValue(sql, new { appId, score = metacriticScore.Score, url = metacriticScore.Url });
        }
    }
}