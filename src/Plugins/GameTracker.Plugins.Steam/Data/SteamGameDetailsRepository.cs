using GameTracker.Plugins.Steam.Helpers;
using GameTracker.Plugins.Steam.Interfaces.Data;
using GameTracker.Plugins.Steam.Models.StoreApi;

namespace GameTracker.Plugins.Steam.Data
{
    public class SteamGameDetailsRepository : DapperRepository<SteamGameDetails>, ISteamGameDetailsRepository
    {
        private const string TableName = "game_details";
        private const string BootstrapSql = @"CREATE TABLE game_details (
                                                appId INTEGER NOT NULL UNIQUE,                                                
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

        private readonly ICategoryRepository _categoryRepository;
        private readonly IDeveloperRepository _developerRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMetacriticScoreRepository _metacriticRepository;
        private readonly IPlatformsRepository _platformsRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IReleaseDateRepository _releaseDateRepository;

        public SteamGameDetailsRepository(ICategoryRepository categoryRepository,
                                          IDeveloperRepository developerRepository,
                                          IGenreRepository genreRepository,
                                          IMetacriticScoreRepository metacriticRepository,
                                          IPlatformsRepository platformsRepository,
                                          IPublisherRepository publisherRepository,
                                          IReleaseDateRepository releaseDateRepository)
            : base(Constants.SQLite.ConnectionString, TableName, BootstrapSql)
        {
            _categoryRepository = categoryRepository;
            _developerRepository = developerRepository;
            _genreRepository = genreRepository;
            _metacriticRepository = metacriticRepository;
            _platformsRepository = platformsRepository;
            _publisherRepository = publisherRepository;
            _releaseDateRepository = releaseDateRepository;
        }

        public async Task<SteamGameDetails?> GetGameDetails(int appId)
        {
            var sql = "SELECT * FROM game_details WHERE appId = @appId";
            var result = (await GetValue(sql, new { appId })).FirstOrDefault();

            if (result != null)
            {
                result.Categories = await _categoryRepository.GetCategories(appId);
                result.Developers = await _developerRepository.GetDevelopers(appId);
                result.Genres = await _genreRepository.GetGenres(appId);
                result.Metacritic = await _metacriticRepository.GetMetacriticScore(appId);
                result.Platforms = await _platformsRepository.GetPlatforms(appId);
                result.Publishers = await _publisherRepository.GetPublishers(appId);
                result.ReleaseDate = await _releaseDateRepository.GetReleaseDate(appId);
            }

            return result;
        }

        public async Task SetGameDetails(SteamGameDetails steamGameDetails)
        {
            var sql = @"INSERT INTO game_details (type, name, appId, isFree, description, about, shortDescription, languages, headerImage, website)
                        VALUES (@type, @name, @appId, @isFree, @description, @about, @shortDescription, @languages, @headerImage, @website)
                        ON CONFLICT DO NOTHING";

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

            if (steamGameDetails.Categories != null && steamGameDetails.Categories.Any())
            {
                await _categoryRepository.SetCategories(steamGameDetails.AppId, steamGameDetails.Categories);
            }

            if (steamGameDetails.Developers != null && steamGameDetails.Developers.Any())
            {
                await _developerRepository.SetDevelopers(steamGameDetails.AppId, steamGameDetails.Developers);
            }

            if (steamGameDetails.Genres != null && steamGameDetails.Genres.Any())
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

            if (steamGameDetails.Publishers != null && steamGameDetails.Publishers.Any())
            {
                await _publisherRepository.SetPublishers(steamGameDetails.AppId, steamGameDetails.Publishers);
            }

            if (steamGameDetails.ReleaseDate != null)
            {
                await _releaseDateRepository.SetReleaseDate(steamGameDetails.AppId, steamGameDetails.ReleaseDate);
            }
        }
    }
}