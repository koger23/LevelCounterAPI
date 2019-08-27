using LevelCounter.Exceptions;
using LevelCounter.Models;
using LevelCounter.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IAccountService accountService;
        private  readonly ApplicationContext context;

        public StatisticsService(IAccountService accountService, ApplicationContext context)
        {
            this.accountService = accountService;
            this.context = context;
        }

        public Task<Statistics> GetStatisticsById(int statisticId)
        {
            return context.Statistics.Where(s => s.StatisticsId == statisticId).SingleAsync();
        }

        public async Task<Statistics> GetUserStatistics(string userId)
        {
            var user = await accountService.FindByIdAsync(userId);
            return await context.Statistics
                .Include(s => s.ApplicationUser)
                .SingleOrDefaultAsync(s => s.StatisticsId == user.StatisticsId) ?? throw new ItemNotFoundException();
        }

        public async Task UpdateStatistics(string userId, Statistics statistics)
        {
            var userStat = await GetUserStatistics(userId);
            UpdateStatisticsProperties(statistics, userStat);
            context.Update(userStat);
            await context.SaveChangesAsync();
        }

        private Statistics UpdateStatisticsProperties(Statistics dtoStats, Statistics userStats)
        {
            userStats.PlayTime = userStats.PlayTime <= dtoStats.PlayTime ? dtoStats.PlayTime : userStats.PlayTime;
            userStats.GamesPlayed = userStats.GamesPlayed <= dtoStats.GamesPlayed ? dtoStats.GamesPlayed : userStats.GamesPlayed;
            userStats.RoundsPlayed = userStats.RoundsPlayed <= dtoStats.RoundsPlayed ? dtoStats.RoundsPlayed : userStats.RoundsPlayed;
            userStats.Wins = userStats.Wins <= dtoStats.Wins ? dtoStats.Wins : userStats.Wins;
            return userStats;
        }
    }
}
