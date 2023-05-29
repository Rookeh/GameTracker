using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Data
{
    public class SteamGameDetailsRepository : DapperRepository<SteamGameDetails>
    {
        private const string TableName = "game_details";
        private const string BootstrapSql = @"CREATE TABLE game_details (
                                                appId INTEGER NOT NULL,                                                
                                                name VARCHAR(1000) NOT NULL,
                                                type VARCHAR(1000) NULL,
                                                isFree INTEGER NULL,
                                                description VARCHAR(1000) NULL,
                                                about VARCHAR(1000) NULL,
                                                shortDescription VARCHAR(1000) NULL,
                                                languages VARCHAR(1000) NULL,
                                                headerImage VARCHAR(1000) NULL,
                                                website VARCHAR(1000) NULL
                                              );";

        private readonly CategoryRepository _categoryRepository;
        private readonly DeveloperRepository _developerRepository;
        private readonly GenreRepository _genreRepository;
        private readonly MetacriticScoreRepository _metacriticRepository;
        private readonly PlatformsRepository _platformsRepository;
        private readonly ReleaseDateRepository _releaseDateRepository;

        public SteamGameDetailsRepository()
            : base(Constants.SteamGameDetails.ConnectionString, TableName, BootstrapSql)
        {
            _categoryRepository = new CategoryRepository();
            _developerRepository = new DeveloperRepository();
            _genreRepository = new GenreRepository();
            _metacriticRepository = new MetacriticScoreRepository();
            _platformsRepository = new PlatformsRepository();
            _releaseDateRepository = new ReleaseDateRepository();
        }

        public async Task<SteamGameDetails?> GetGameDetails(int appId)
        {
            // TODO: Bootstrap database if it does not exist on first run.

            var sql = "SELECT * FROM game_details WHERE appId = @appId";
            var result = (await GetValue(sql, new { appId })).FirstOrDefault();

            if (result != null)
            {
                result.Categories = await _categoryRepository.GetCategories(appId);
                result.Developers = await _developerRepository.GetDevelopers(appId);
                result.Genres = await _genreRepository.GetGenres(appId);
                result.Metacritic = await _metacriticRepository.GetMetacriticScore(appId);
                result.Platforms = await _platformsRepository.GetPlatforms(appId);
                result.ReleaseDate = await _releaseDateRepository.GetReleaseDate(appId);
            }

            return result;
        }

        public async Task SetGameDetails(SteamGameDetails steamGameDetails)
        {
            var sql = @"INSERT INTO game_details (type, name, appId, isFree, description, about, shortDescription, languages, headerImage, website)
                        VALUES (@type, @name, @appId, @isFree, @description, @about, @shortDescription, @languages, @headerImage, @website)";

            await SetValue(sql, new
            {
                type = steamGameDetails.Type,
                name = steamGameDetails.Name,
                appId = steamGameDetails.AppId,
                isFree = steamGameDetails.IsFree,
                description = steamGameDetails.Description,
                about = steamGameDetails.About,
                shortDescription = steamGameDetails.ShortDescription,
                languages = steamGameDetails.Languages,
                headerImage = steamGameDetails.HeaderImage,
                website = steamGameDetails.Website
            });

            if (steamGameDetails.Categories != null)
            {
                await _categoryRepository.SetCategories(steamGameDetails.AppId, steamGameDetails.Categories);
            }

            if (steamGameDetails.Developers != null)
            {
                await _developerRepository.SetDevelopers(steamGameDetails.AppId, steamGameDetails.Developers);
            }

            if (steamGameDetails.Genres != null)
            {
                await _genreRepository.SetGenres(steamGameDetails.AppId, steamGameDetails.Genres);
            }

            if (steamGameDetails.Metacritic != null)
            {
                await _metacriticRepository.SetMetacriticScore(steamGameDetails.AppId, steamGameDetails.Metacritic);
            }

            if (steamGameDetails.Platforms != null)
            {
                await _platformsRepository.SetPlatforms(steamGameDetails.AppId, steamGameDetails.Platforms);
            }

            if (steamGameDetails.ReleaseDate != null)
            {
                await _releaseDateRepository.SetReleaseDate(steamGameDetails.AppId, steamGameDetails.ReleaseDate);
            }
        }
    }
}