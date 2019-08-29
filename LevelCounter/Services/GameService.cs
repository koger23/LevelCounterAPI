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

        public async Task<GameViewModel> CreateGame(NewGameRequest newGameRequest, string userId)
        {
            var user = context.Users.Where(u => u.Id == userId).SingleOrDefault();
            if (user == null)
            {
                throw new ItemNotFoundException();
            }
            var game = new Game
            {
                ApplicationUser = user,
                ApplicationUserId = userId,
                InGameUsers = await GetInGameUsers(await GetUsersByUserNameAsync(newGameRequest.UserNames))
            };
            var gameViewModel = new GameViewModel();
            return gameViewModel;
        }

        private async Task<List<InGameUser>> GetInGameUsers(List<ApplicationUser> users)
        {
            var inGameUsers = new List<InGameUser>();
            var result = await Task.Run(() =>
            {
                foreach (var user in users)
                {
                    inGameUsers.Add(new InGameUser()
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Sex = user.Sex,
                        Gender = user.Gender
                    });
                }
                return inGameUsers;
            });
            return result;
        }

        private async Task<List<ApplicationUser>> GetUsersByUserNameAsync(List<string> userNames)
        {
            var users = new List<ApplicationUser>();
            var result = await Task.Run(() =>
            {
                foreach (var username in userNames)
                {
                    var user = context.Users.Where(u => u.UserName == username).SingleOrDefault();
                    if (user != null)
                    {
                        users.Add(user);
                    }
                }
                return users;
            });
            return result;
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
