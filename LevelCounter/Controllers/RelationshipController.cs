using LevelCounter.Exceptions;
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
    public class RelationshipController : ControllerBase
    {
        private readonly IRelationshipService relationshipService;
        private const string authScheme = JwtBearerDefaults.AuthenticationScheme;

        public RelationshipController(IRelationshipService relationshipService)
        {
            this.relationshipService = relationshipService;
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPost("connect")]
        public async Task<IActionResult> MakeConnectionRequest([FromQuery(Name = "userName")] string userName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var newRelation = await relationshipService.MakeFriendRequest(userName, userId);
                return Ok(newRelation);
            } catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPost("block")]
        public async Task<IActionResult> BlockUser([FromQuery(Name = "userName")] string userName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await relationshipService.BlockUser(userName, userId);
                return Ok();
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
