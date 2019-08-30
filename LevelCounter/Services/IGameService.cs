using LevelCounter.Models;
using LevelCounter.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public interface IGameService
    {
        Task<Game> CreateGameAsync(string userId);
        Task<Game> AddInGameUsersAsync(NewGameRequest gameRequest, string userId);
        Task UpdateInGameUserAsync(UpdateInGameUserRequest updateInGameUserRequest, string userId);
        Task<Game> LoadGameAsync(int gameId, string userId);
        Task SaveGame(Game game, string userId);
        Task<Game> StartGameAsync(int gameId, string userId);
        Task<Game> QuitGameAsync(int gameId, string userId);
        bool CheckHostId(int gameId, string userId);
        Task DeleteGameAsync(int gameId, string userId);
        Task<List<Game>> GetHostedGames(string userId);
        Task<List<Game>> GetRelatedGames(string userId);
    }
}
