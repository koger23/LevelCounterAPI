using AutoMapper;
using LevelCounter.Exceptions;
using LevelCounter.Models;
using LevelCounter.Models.DTO;
using LevelCounter.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationContext context;
        private readonly IMapper mapper;

        public GameService(ApplicationContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Game> CreateGameAsync(string userId)
        {
            var user = context.Users.Where(u => u.Id == userId).SingleOrDefault();
            if (user == null)
            {
                throw new ItemNotFoundException();
            }
            var game = new Game
            {
                HostingUserId = userId
            };
            var savedGame = context.Games.Add(game);
            await context.SaveChangesAsync();
            return game;
        }

        public async Task<Game> AddInGameUsersAsync(NewGameRequest gameRequest, string userId)
        {
            var game = context.Games.Where(g => g.Id == gameRequest.gameId).SingleOrDefault();
            if (game != null && game.HostingUserId == userId)
            {
                await CreateInGameUsersBasedOnUserNameAsync(gameRequest.UserNames, game);
                context.Update(game);
                await context.SaveChangesAsync();
                return game;
            }
            throw new ItemNotFoundException();
        }

        public async Task UpdateInGameUserAsync(UpdateInGameUserRequest updateInGameUserRequest, string userId)
        {
            var game = context.Games.Where(g => g.Id == updateInGameUserRequest.GameId).SingleOrDefault() ?? throw new ItemNotFoundException();
            var inGameUser = context.InGameUsers.Where(i => i.InGameUserId == updateInGameUserRequest.InGameUserId).SingleOrDefault() ?? throw new ItemNotFoundException();
            if (game.HostingUserId == userId)
            {
                inGameUser.Level = updateInGameUserRequest.Level <= 1 ? 1 : updateInGameUserRequest.Level;
                inGameUser.Bonus = updateInGameUserRequest.Bonus <= 0 ? 0 : updateInGameUserRequest.Bonus;
                context.InGameUsers.Update(inGameUser);
                await context.SaveChangesAsync();
            }
        }

        private async Task<List<InGameUser>> CreateInGameUsersBasedOnUserNameAsync(List<string> userNames, Game game)
        {
            var users = new List<InGameUser>();
            foreach (var username in userNames)
            {
                var user = context.Users.Where(u => u.UserName == username).SingleOrDefault();
                var inGameUserCheck = context.InGameUsers.Where(i => i.GameId == game.Id).Where(i => i.UserName == username).SingleOrDefault();
                if (user != null && inGameUserCheck == null)
                {
                    var inGameUser = new InGameUser
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        GameId = game.Id
                    };
                    users.Add(inGameUser);
                    await context.InGameUsers.AddAsync(inGameUser);
                }
            }
            return users;
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
