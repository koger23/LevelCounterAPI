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

        [HttpGet]
        public async Task GetGameState([FromQuery] int gameid)
        {
            var context = ControllerContext.HttpContext;
            var isSocketRequest = context.WebSockets.IsWebSocketRequest;

            if (isSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine(gameid);
                await GetMessages(webSocket);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateNewGame([FromBody] NewGameRequest newGameRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (newGameRequest.UserNames.Count > 0)
            {
                try
                {
                    var game = await gameService.CreateGame(newGameRequest, userId);
                    return Ok(game);
                } catch (ItemNotFoundException e)
                {
                    return BadRequest(e.Message);
                }
            } else
            {
                return BadRequest("Number of players must be higher than 0");
            }
        }

        private async Task GetMessages(WebSocket webSocket)
        {
            var objectToSend = new UserShortResponse
            {
                UserName = "server",
                StatisticsId = 13
            };
            var objList = new List<UserShortResponse>()
            {
                objectToSend,
                objectToSend,
                objectToSend,
                objectToSend
            };
            var jsonUser = JsonConvert.SerializeObject(objList);
            var bytes = Encoding.ASCII.GetBytes(jsonUser);
            var arraySegment = new ArraySegment<byte>(bytes);
            await Echo(webSocket, arraySegment);
        }
        private async Task Echo(WebSocket webSocket, ArraySegment<byte> message)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(message, result.MessageType, result.EndOfMessage, CancellationToken.None);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
