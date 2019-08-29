using LevelCounter.Exceptions;
using LevelCounter.Models;
using LevelCounter.Models.DTO;
using LevelCounter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationContext context;

        public GameService(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<Game> CreateGame(NewGameRequest newGameRequest, string userId)
        {
            var inGameUsers = new List<ApplicationUser>();

            foreach (var userName in newGameRequest.UserNames)
            {
                var user = context.Users.Where(u => u.UserName == userName).SingleOrDefault();
                if (user != null)
                {
                    inGameUsers.Add(user);
                } else
                {
                    throw new ItemNotFoundException();
                }
            }
            var game = new Game
            {
                ApplicationUserId = userId,
                InGameUsers = inGameUsers,
                Time = 0
            };
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();
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
