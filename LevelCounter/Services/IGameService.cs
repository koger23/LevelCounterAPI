using LevelCounter.Models;
using LevelCounter.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public interface IGameService
    {
        Task<Game> CreateGame(string userId);
        Task<Game> AddInGameUsers(NewGameRequest gameRequest, string userId);
        Task UpdateInGameUserAsync(UpdateInGameUserRequest updateInGameUserRequest, string userId);
        Game LoadGame(int gameId);
        Game SaveGame(Game game);
        Game FinishGame(Game game);
    }
}
