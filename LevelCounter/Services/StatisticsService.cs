using LevelCounter.Exceptions;
using LevelCounter.Models;
using LevelCounter.Repository;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Statistics> GetUserStatistics(string userId)
        {
            var user = await accountService.FindByIdAsync(userId);
            return await context.Statistics
                .Include(s => s.ApplicationUser)
                .SingleOrDefaultAsync(s => s.StatisticsId == user.StatisticsId) ?? throw new ItemNotFoundException();
        }
    }
}
