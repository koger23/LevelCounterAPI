using LevelCounter.Models;
using LevelCounter.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public interface IGameService
    {
        Task<Game> CreateGameAsync(string userId);
        Task<List<InGameUser>> GetInGameUsersByGameId(int gameId);
        Task<Game> AddInGameUsersAsync(NewGameRequest gameRequest, string userId);
        Task UpdateInGameUserAsync(UpdateInGameUserRequest updateInGameUserRequest, string userId);
        Task<Game> LoadGameAsync(int gameId, string userId);
        Task SaveGame(Game game, string userId);
        Task<Game> StartGameAsync(int gameId, string userId);
        Task QuitGameAsync(int gameId, string userId);
        bool CheckHostId(int gameId, string userId);
        Task DeleteGameAsync(int gameId, string userId);
        Task<List<Game>> GetHostedGames(string userId);
        Task<List<Game>> GetRelatedGames(string userId);
        Task<Game> JoinGame(int gameId, string userId);
        Task UpdateGame(Game game, string userId);
    }
}
