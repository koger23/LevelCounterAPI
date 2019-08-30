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
        Game LoadGame(int gameId);
        Game SaveGame(Game game);
        Game FinishGame(Game game);
    }
}
