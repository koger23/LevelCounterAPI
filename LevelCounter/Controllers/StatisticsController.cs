using LevelCounter.Exceptions;
using LevelCounter.Models;
using LevelCounter.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LevelCounter.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService statisticsService;
        private const string authScheme = JwtBearerDefaults.AuthenticationScheme;

        public StatisticsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("userStats")]
        public async Task<IActionResult> GetUserStatistics()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var statistics = await statisticsService.GetUserStatistics(userId);
                return Ok(statistics);
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateStatistics([FromBody] Statistics statistics)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await statisticsService.UpdateStatistics(userId, statistics);
            return Ok("Statistics saved");
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStatisticsById(int id)
        {
            var stat = await statisticsService.GetStatisticsById(id);
            return Ok(stat);
        }
    }
}