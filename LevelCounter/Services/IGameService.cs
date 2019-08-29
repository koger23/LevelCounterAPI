using LevelCounter.Models;
using LevelCounter.Models.DTO;
using System;
using System.Collections.Generic;

namespace LevelCounter.Services
{
    public interface IGameService
    {
        Game CreateGame(List<InGameUser> inGameUsers, string userId);
        Game LoadGame(int gameId);
        Game SaveGame(Game game);
        Game FinishGame(Game game);
    }
}
