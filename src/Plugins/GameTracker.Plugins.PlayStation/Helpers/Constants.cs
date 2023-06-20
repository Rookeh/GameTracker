using GameTracker.Models;
using GameTracker.Models.Enums;

namespace GameTracker.Plugins.PlayStation.Helpers
{
    public static class Constants
    {
        public static class Authentication
        {
            public const string AuthenticationBaseUrl = "https://ca.account.sony.com/api/authz/v3/oauth";
            public const string ClientId = "09515159-7237-4370-9b40-3806e67c0891";
            public const string TokenExchangeCredential = "MDk1MTUxNTktNzIzNy00MzcwLTliNDAtMzgwNmU2N2MwODkxOnVjUGprYTV0bnRCMktxc1A=";
            public const string RedirectUri = "com.scee.psxandroid.scecompcall://redirect";
            public const string Scope = "psn:mobile.v2.core psn:clientapp";
            public const string UserLoginLink = "https://www.playstation.com/";
            public const string NPSSOTokenLink = "https://ca.account.sony.com/api/v1/ssocookie";
        }

        public static class Consoles
        {
            public const string PS3 = "PS3";
            public const string PS4 = "PS4";
            public const string PS5 = "PS5";
        }

        public static class ConsolePlatforms
        {
            public static Platform PlayStation3 => new Platform()
            {
                Name = "PlayStation 3",
                Icon = "Playstation",
                Description = "The PlayStation 3 is a home video game console developed and marketed by Sony Interactive Entertainment. The successor to the PlayStation 2, it is part of the PlayStation brand of consoles."
            };

            public static Platform PlayStation4 => new Platform()
            {
                Name = "PlayStation 4",
                Icon = "Playstation",
                Description = "The PlayStation 4 is a home video game console developed by Sony Interactive Entertainment. Announced as the successor to the PlayStation 3 in February 2013, it was launched on November 15, 2013, in North America, November 29, 2013 in Europe, South America and Australia, and on February 22, 2014 in Japan."
            };

            public static Platform PlayStation5 => new Platform()
            {
                Name = "PlayStation 5",
                Icon = "Playstation",
                Description = "The PlayStation 5 is a home video game console developed by Sony Interactive Entertainment. It was announced as the successor to the PlayStation 4 in April 2019, was launched on November 12, 2020, in Australia, Japan, New Zealand, North America, and South Korea, and was released worldwide one week later."
            };
        }

        public static class Mappings
        {
            public static Dictionary<string, Genre> PSNGenreMappings = new Dictionary<string, Genre>
            {
                ["Action"] = Genre.Action,
                ["Adventure"] = Genre.Adventure,
                ["Adult"] = Genre.Other,
                ["Arcade"] = Genre.Arcade,
                ["Brain Training"] = Genre.Other,
                ["Casual"] = Genre.Casual,
                ["Driving/Racing"] = Genre.Racing,
                ["Educational"] = Genre.Puzzle,
                ["Family"] = Genre.Casual,
                ["Fighting"] = Genre.Fighting,
                ["Fitness"] = Genre.Other,
                ["Horror"] = Genre.Horror,
                ["Music/Rhythm"] = Genre.Music,
                ["Party"] = Genre.Party,
                ["Puzzle"] = Genre.Puzzle,
                ["Quiz"] = Genre.Trivia,
                ["Role Playing Games"] = Genre.RPG,
                ["Shooter"] = Genre.Shooter,
                ["Simulation"] = Genre.Simulation,                
                ["Sport"] = Genre.Sports,
                ["Strategy"] = Genre.Strategy,
                ["Unique"] = Genre.Other
            };
        }

        public static class GraphQL
        {
            // public const string StoreDetailsEndpoint = "https://web.np.playstation.com/api/graphql/v1//op?operationName=queryRetrieveTelemetryDataPDPProduct&variables={\"conceptId\":null,\"productId\":\"{0}\"}&extensions={\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"07746553f497a2ef237b7b3a5e7a971e21f2b1f5dcb9df2d038ded681daee79c\"}}";

            public const string GraphQLBaseUrl = "https://web.np.playstation.com/api/graphql/v1/op";
            public const string GetUserGameOperation = "getUserGameList";
            public const string GetUserGameFilter = "[\"ps4_game\", \"ps5_native_game\"]";
            public const string GetUserGameListHash = "e780a6d8b921ef0c59ec01ea5c5255671272ca0d819edb61320914cf7a78b3ae";
            public const string GameType = "GameLibraryTitle";
            public const string GetStoreDetailsOperation = "queryRetrieveTelemetryDataPDPProduct";
            public const string GetStoreDetailsQueryFormat = "{{\"conceptId\":null,\"productId\":\"{0}\"}}";
            public const string GetStoreDetailsExtensions = "{\"persistedQuery\":{\"version\":1,\"sha256Hash\":\"07746553f497a2ef237b7b3a5e7a971e21f2b1f5dcb9df2d038ded681daee79c\"}}";
        }

        public static class LaunchCommands
        {
            public const string Media = "https://www.playstation.com/{0}/{1}/{1}-entertainment/";
            public const string Game = "https://store.playstation.com/{0}/product/{1}";
        }
    }
}