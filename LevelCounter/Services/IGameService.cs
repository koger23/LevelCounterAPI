using LevelCounter.Models;
using System.Collections.Generic;

namespace LevelCounter.Services
{
    public interface IGameService
    {
        Game CreateGame(List<string> userNames, string userId);
        Game LoadGame(int gameId);
        Game SaveGame(Game game);
        Game FinishGame(Game game);
    }
}
