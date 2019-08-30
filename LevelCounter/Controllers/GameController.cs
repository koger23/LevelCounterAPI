using LevelCounter.Exceptions;
using LevelCounter.Models.DTO;
using LevelCounter.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LevelCounter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private const string authScheme = JwtBearerDefaults.AuthenticationScheme;
        private readonly IGameService gameService;

        public GameController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("gameState")]
        public async Task GetGameState([FromQuery] int gameid, string userid)
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (appUserId == userid)
            {

            }

        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewGame()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                return new ObjectResult(await gameService.CreateGame(userId))
                {
                    StatusCode = 201
                };
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (FormatException e)
            {
                return BadRequest(e.Message);
            }

        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPost("addInGameUsers")]
        public async Task<IActionResult> AddInGameUsers([FromBody] NewGameRequest newGameRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (newGameRequest.UserNames.Count > 0)
            {
                try
                {
                    return new ObjectResult(await gameService.AddInGameUsers(newGameRequest, userId))
                    {
                        StatusCode = 201
                    };
                }
                catch (ItemNotFoundException e)
                {
                    return BadRequest(e.Message);
                }
                catch (FormatException e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return BadRequest("Number of players must be higher than 0");
            }
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPost("updateInGameUser")]
        public async Task<IActionResult> UpdateInGameUser([FromBody] UpdateInGameUserRequest updateInGameUserRequest)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await gameService.UpdateInGameUserAsync(updateInGameUserRequest, userId);
                return Ok("User updated");
            } catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
