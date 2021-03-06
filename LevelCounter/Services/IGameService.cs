﻿using LevelCounter.Models;
using LevelCounter.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public interface IGameService
    {
        Task<Game> CreateGameAsync(string userId);
        List<InGameUser> GetInGameUsersByGameIdAsync(int gameId);
        Task<Game> AddInGameUsersAsync(NewGameRequest gameRequest, string userId);
        Task UpdateInGameUserAsync(UpdateInGameUserRequest updateInGameUserRequest, string userId);
        Task<Game> LoadGameAsync(int gameId, string userId);
        Task SaveGameAsync(Game game, string userId);
        Task<Game> StartGameAsync(int gameId, string userId);
        Task QuitGameAsync(Game game, string userId);
        bool CheckHostId(int gameId, string userId);
        Task DeleteGameAsync(int gameId, string userId);
        List<Game> GetHostedGames(string userId);
        List<Game> GetRelatedGames(string userId);
        Task<Game> JoinGame(int gameId, string userId);
        Task UpdateGame(Game game, string userId);
    }
}
