using LevelCounter.Exceptions;
using LevelCounter.Models;
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
        [HttpPost("startGame")]
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
            return BadRequest();

        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateNewGame()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                return Ok(await gameService.CreateGameAsync(userId));
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
        [HttpGet("getPlayers")]
        public IActionResult CreateNewGame([FromQuery] int gameId)
        {
            try
            {
                var list = gameService.GetInGameUsersByGameIdAsync(gameId);
                if (list.Count > 0) return Ok(list);
                return BadRequest("Invalid game id");
            }
            catch (Exception e)
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
                    return Ok(await gameService.AddInGameUsersAsync(newGameRequest, userId));
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
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (HostMisMatchException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPut("saveGame")]
        public async Task<IActionResult> SaveGame([FromBody] Game game)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await gameService.SaveGameAsync(game, userId);
                return Ok("Game saved");
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (HostMisMatchException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPost("quitGame")]
        public async Task<IActionResult> QuitGame([FromBody] Game game)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (gameService.CheckHostId(game.Id, userId))
            {
                try
                {
                    await gameService.QuitGameAsync(game, userId);
                    return Ok();
                }
                catch (ItemNotFoundException e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest();

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
            catch (MissingInGameUserException)
            {
                return NoContent();
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
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("savedGames")]
        public IActionResult ListSavedGames()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(gameService.GetHostedGames(userId));
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("joinableGames")]
        public IActionResult ListJoinableGames()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(gameService.GetRelatedGames(userId));
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("joinGame")]
        public async Task<IActionResult> JoinGame([FromQuery] int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                return Ok(await gameService.JoinGame(gameId, userId));
            } catch(MissingInGameUserException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPut("updateGame")]
        public async Task<IActionResult> UpdateGame([FromBody] Game game)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await gameService.UpdateGame(game, userId);
                return Ok();
            } catch(MissingInGameUserException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
