using GameTracker.Models.Enums;
using GameTracker.Plugins.IGDB.Models;

namespace GameTracker.Plugins.IGDB.Helpers
{
    public static class EnumHelper
    {
        public static GameplayMode ToGameplayMode(this int gameplayMode)
        {
            var igdbMode = (IGDBGameMode)gameplayMode;

            switch (igdbMode)
            {
                case IGDBGameMode.BattleRoyale:
                case IGDBGameMode.Multiplayer:
                case IGDBGameMode.SplitScreen:
                    return GameplayMode.PvP;
                case IGDBGameMode.CoOp: 
                    return GameplayMode.CoOp;
                default:
                    return GameplayMode.Singleplayer;
            }
        }

        public static Genre ToGenre(this int genre)
        {
            var igdbGenre = (IGDBGenre)genre;

            switch (igdbGenre)
            {
                case IGDBGenre.Adventure: 
                    return Genre.Adventure;
                case IGDBGenre.Arcade: 
                    return Genre.Arcade;
                case IGDBGenre.BoardGame: 
                    return Genre.BoardGame;                
                case IGDBGenre.Fighting:
                case IGDBGenre.HackNSlash:
                    return Genre.Fighting;
                case IGDBGenre.Indie: 
                    return Genre.Indie;
                case IGDBGenre.MOBA:
                    return Genre.MOBA;
                case IGDBGenre.Music:
                    return Genre.Music;
                case IGDBGenre.Pinball:
                    return Genre.Other;
                case IGDBGenre.Platform:
                    return Genre.Platformer;
                case IGDBGenre.PointNClick:
                    return Genre.Other;
                case IGDBGenre.Puzzle: 
                    return Genre.Puzzle;
                case IGDBGenre.Racing:
                    return Genre.Racing;
                case IGDBGenre.RPG: 
                    return Genre.RPG;
                case IGDBGenre.RTS:
                    return Genre.RTS;
                case IGDBGenre.Shooter: 
                    return Genre.Shooter;
                case IGDBGenre.Simulation: 
                    return Genre.Simulation;
                case IGDBGenre.Sport:
                    return Genre.Sports;                
                case IGDBGenre.Strategy:
                case IGDBGenre.Tactical:
                case IGDBGenre.TurnBasedStrategy:
                    return Genre.Strategy;
                case IGDBGenre.Trivia: 
                    return Genre.Trivia;
                case IGDBGenre.VisualNovel:
                    return Genre.VisualNovel;
                default:
                    return Genre.Other;
            }
        }
    }
}
