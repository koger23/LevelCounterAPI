using LevelCounter.Exceptions;
using LevelCounter.Models;
using LevelCounter.Models.DTO;
using LevelCounter.Repository;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Game> CreateGameAsync(string userId)
        {
            var user = context.Users.Where(u => u.Id == userId).SingleOrDefault() ?? throw new ItemNotFoundException();
            var game = new Game
            {
                HostingUserId = userId
            };
            var savedGame = context.Games.Add(game);
            await context.SaveChangesAsync();
            return game;
        }

        public async Task<List<InGameUser>> GetInGameUsersByGameId(int gameId)
        {
            return await context.InGameUsers.Where(u => u.GameId == gameId).ToListAsync();
        }

        public async Task<Game> AddInGameUsersAsync(NewGameRequest gameRequest, string userId)
        {
            gameRequest.UserNames.Add(context.Users.Where(u => u.Id == userId).SingleOrDefault().UserName);
            var game = context.Games.Where(g => g.Id == gameRequest.gameId).SingleOrDefault() ?? throw new ItemNotFoundException();
            if (game.HostingUserId == userId)
            {
                await CreateInGameUsersBasedOnUserNameAsync(gameRequest.UserNames, game);
                context.Update(game);
                await context.SaveChangesAsync();
                return game;
            }
            throw new HostMisMatchException();
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
            else
            {
                throw new HostMisMatchException();
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
                        GameId = game.Id,
                        Gender = user.Gender
                    };
                    users.Add(inGameUser);
                    await context.InGameUsers.AddAsync(inGameUser);
                }
            }
            return users;
        }

        public async Task<Game> StartGameAsync(int gameId, string userId)
        {
            var game = context.Games.Where(g => g.Id == gameId).Include(g => g.InGameUsers).SingleOrDefault() ?? throw new ItemNotFoundException();
            if (game.HostingUserId == userId)
            {
                game.IsRunning = true;
                context.Games.Update(game);
                await context.SaveChangesAsync();
            }
            return game;
        }

        public async Task QuitGameAsync(int gameId, string userId)
        {
            var game = context.Games.Where(g => g.Id == gameId).SingleOrDefault() ?? throw new ItemNotFoundException();
            if (game.HostingUserId == userId)
            {
                context.Games.Remove(game);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Game> LoadGameAsync(int gameId, string userId)
        {
            var game = await context.Games.Where(g => g.Id == gameId)
                .Include(i => i.InGameUsers)
                .SingleOrDefaultAsync()
                ?? throw new ItemNotFoundException();
            if (CheckInGameUserInGameExists(game.InGameUsers, userId))
            {
                game.IsRunning = true;
                context.Games.Update(game);
                await context.SaveChangesAsync();
                return game;
            }
            throw new MissingInGameUserException();
        }

        private bool CheckInGameUserInGameExists(List<InGameUser> inGameUsers, string userId)
        {
            bool result = false;
            foreach (var user in inGameUsers)
            {
                if (user.UserId == userId)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public async Task SaveGame(Game game, string userId)
        {
            if (game.HostingUserId == userId)
            {
                var gameFromDb = context.Games.Where(g => g.Id == game.Id).Include(g => g.InGameUsers).FirstOrDefault() ?? throw new ItemNotFoundException();
                gameFromDb.Time = game.Time;
                gameFromDb.IsRunning = false;
                game.InGameUsers.ForEach(i =>
                {
                    var user = gameFromDb.InGameUsers.Where(u => u.InGameUserId == i.InGameUserId).FirstOrDefault() ?? throw new ItemNotFoundException();
                    user.Level = i.Level;
                    user.Bonus = i.Bonus;
                }
                );
                context.Games.Update(gameFromDb);
                await context.SaveChangesAsync();
                Console.WriteLine("***********Game saved.");
            }
            throw new HostMisMatchException();
        }

        public bool CheckHostId(int gameId, string userId)
        {
            var game = context.Games.Where(g => g.Id == gameId).SingleOrDefault() ?? throw new ItemNotFoundException();
            return game.HostingUserId == userId ? true : false;
        }

        public async Task DeleteGameAsync(int gameId, string userId)
        {
            var game = context.Games.Where(g => g.Id == gameId).SingleOrDefault() ?? throw new ItemNotFoundException();
            if (game.HostingUserId != userId) throw new HostMisMatchException();
            context.Games.Remove(game);
            await context.SaveChangesAsync();
        }

        public async Task<List<Game>> GetHostedGames(string userId)
        {
            var games = await Task.Run(() =>
            {
                return context.Games
                    .Include(g => g.InGameUsers)
                    .Where(g => g.HostingUserId == userId)
                    .Where(g => CheckInGameUserInGameExists(g.InGameUsers, userId))
                    .ToList()
                    ?? throw new ItemNotFoundException();
            });
            return games;
        }

        public async Task<List<Game>> GetRelatedGames(string userId)
        {
            var games = await Task.Run(() =>
            {
                return context.Games
                .Include(g => g.InGameUsers)
                .Where(g => g.HostingUserId != userId)
                .Where(g => g.IsRunning == true)
                .Where(g => CheckInGameUserInGameExists(g.InGameUsers, userId))
                .ToList()
                ?? throw new ItemNotFoundException();
            });
            return games;
        }

        public async Task<Game> JoinGame(int gameId, string userId)
        {
            var savedGame = await Task.Run(() =>
            {
                var game = context.Games
                .Where(g => g.Id == gameId)
                .Include(g => g.InGameUsers)
                .FirstOrDefault()
                ?? throw new ItemNotFoundException();
                if (game.IsRunning && CheckInGameUserInGameExists(game.InGameUsers, userId))
                {
                    return game;
                }
                else
                {
                    throw new MissingInGameUserException();
                }
            });
            return savedGame;
        }
    }
}
