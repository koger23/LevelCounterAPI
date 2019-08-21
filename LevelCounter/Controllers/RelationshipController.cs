using LevelCounter.Exceptions;
using LevelCounter.Models;
using LevelCounter.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LevelCounter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelationshipController : ControllerBase
    {
        private readonly IRelationshipService relationshipService;
        private readonly IAccountService accountService;
        private const string authScheme = JwtBearerDefaults.AuthenticationScheme;

        public RelationshipController(IAccountService accountService, IRelationshipService relationshipService)
        {
            this.accountService = accountService;
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
    }
}
