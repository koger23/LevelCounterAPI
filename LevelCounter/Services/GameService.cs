using LevelCounter.Models;
using LevelCounter.Repository;
using System.Collections.Generic;
using System.Linq;

namespace LevelCounter.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationContext context;

        public GameService(ApplicationContext context)
        {
            this.context = context;
        }

        public Game CreateGame(List<string> userNames, string userId)
        {
            var inGameUsers = new List<ApplicationUser>();

            foreach (var userName in userNames)
            {
                inGameUsers.Add(context.Users.Where(u => u.UserName == userName).SingleOrDefault());
            }

            var game = new Game
            {
                ApplicationUserId = userId,
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
