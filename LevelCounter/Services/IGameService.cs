using LevelCounter.Models;
using LevelCounter.Models.DTO;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public interface IGameService
    {
        Task<GameViewModel> CreateGame(NewGameRequest newGameRequest, string userId);
        Game LoadGame(int gameId);
        Game SaveGame(Game game);
        Game FinishGame(Game game);
    }
}
