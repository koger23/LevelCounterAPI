﻿using LevelCounter.Models;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public interface IStatisticsService
    {
        Task<Statistics> GetUserStatistics(string userId);
        Task UpdateStatistics(string userId, Statistics statistics);
        Task<Statistics> GetStatisticsById(int id);
        Task IncreaseGamesPlayedAsync(string userId);
        Task IncreaseWinsAsync(string userId);
    }
}
