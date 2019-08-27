using HotelBookingApp.Models.DTO;
using LevelCounter.Models.DTO;
using LevelCounter.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LevelCounter.Exceptions
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly IUserService userService;
        private readonly IRelationshipService relationshipService;
        private const string authScheme = JwtBearerDefaults.AuthenticationScheme;

        public UserController(IAccountService accountService, IUserService userService, IRelationshipService relationshipService)
        {
            this.accountService = accountService;
            this.userService = userService;
            this.relationshipService = relationshipService;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignupRequest signUpRequest)
        {
            var errors = await accountService.SignUpAsync(signUpRequest);
            if (errors.Errors.Count == 0)
            {
                return new ObjectResult(errors)
                {
                    StatusCode = 201
                };
            }
            return BadRequest(errors);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await accountService.SignInAsync(request);
            if (response.Token == null)
            {
                return BadRequest(response.ErrorMessages);
            }
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserEditRequest userEditRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(userEditRequest);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var errors = await accountService.UpdateAsync(userId, userEditRequest);
            if (errors.Count == 0)
            {
                return Ok("Userdata updated successfully.");
            }
            return BadRequest(errors);
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("userdata")]
        public async Task<IActionResult> GetUserData()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var user = await accountService.FindByIdAsync(userId);
                return Ok(user);
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("userlist")]
        public async Task<IActionResult> GetUserList()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var users = await userService.GetUsersAsync(userId);
            return Ok(users);
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("friends")]
        public IActionResult GetFriends()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = userService.GetFriendsAsync(userId);
            return Ok(user);
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("requests/pending")]
        public IActionResult GetPending()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = userService.GetPendingRequests(userId);
            return Ok(user);
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("requests/blocked")]
        public IActionResult GetBlocked()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = userService.GetBlocked(userId);
            return Ok(user);
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("requests/unconfirmed")]
        public IActionResult GetUnconfirmed()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = userService.GetUnconfirmedAsync(userId);
            return Ok(user);
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPut("requests/{relationshipId}/confirm")]
        public async Task<IActionResult> Confirm(int relationshipId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await relationshipService.ConfirmRequest(relationshipId, userId);
                return Ok("Confirmed");
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
