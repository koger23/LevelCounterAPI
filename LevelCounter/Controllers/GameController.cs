using LevelCounter.Exceptions;
using LevelCounter.Models.DTO;
using LevelCounter.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
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
        [HttpGet("startGame")]
        public async Task<IActionResult> StartGame([FromQuery] int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (gameService.CheckHostId(gameId, userId))
            {
                try
                {
                    return Ok(await gameService.StartGameAsync(gameId, userId));
                }
                catch (ItemNotFoundException e)
                {
                    return BadRequest(e.Message);
                }
            }
            return Forbid();

        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewGame()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                return new ObjectResult(await gameService.CreateGameAsync(userId))
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
                    return new ObjectResult(await gameService.AddInGameUsersAsync(newGameRequest, userId))
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
                catch (HostMisMatchException e)
                {
                    return Forbid(e.Message);
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
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (HostMisMatchException e)
            {
                return Forbid(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("quitGame")]
        public async Task<IActionResult> QuitGame([FromQuery] int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (gameService.CheckHostId(gameId, userId))
            {
                try
                {
                    return Ok(await gameService.QuitGameAsync(gameId, userId));
                }
                catch (ItemNotFoundException e)
                {
                    return BadRequest(e.Message);
                }
            }
            return Forbid();

        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("loadGame")]
        public async Task<IActionResult> LoadGame([FromQuery] int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                return Ok(await gameService.LoadGameAsync(gameId, userId));
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (MissingInGameUserException e)
            {
                return Forbid(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteGame([FromQuery] int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await gameService.DeleteGameAsync(gameId, userId);
                return NoContent();
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (HostMisMatchException e)
            {
                return Forbid(e.Message);
            }
        }
    }
}
