using System.Collections.Generic;
using LevelCounter.Models;
using LevelCounter.Models.DTO;

namespace LevelCounter.Services
{
    public class GameService : IGameService
    {
        public Game CreateGame(List<InGameUser> inGameUsers, string userId)
        {
            var game = new Game
            {
                HostingUserId = userId,
                InGameUsers = inGameUsers,
                Time = 0
            };
            return game;
        }

        public Game FinishGame(Game game)
        {
            throw new System.NotImplementedException();
        }

        public Game LoadGame(int gameId)
        {
            throw new System.NotImplementedException();
        }

        public Game SaveGame(Game game)
        {
            throw new System.NotImplementedException();
        }
    }
}
