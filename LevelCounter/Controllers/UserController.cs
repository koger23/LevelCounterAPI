﻿using HotelBookingApp.Models.DTO;
using LevelCounter.Models.DTO;
using LevelCounter.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LevelCounter.Exceptions
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountService accountService;
        private const string authScheme = JwtBearerDefaults.AuthenticationScheme;

        public UserController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<ActionResult> SignUp([FromBody] SignupRequest signUpRequest)
        {
            var errors = await accountService.SignUpAsync(signUpRequest);
            if (errors.Count == 0)
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
                return new OkObjectResult("Userdata updated successfully.");
            }
            return BadRequest(errors);
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("userdata")]
        public async Task<ObjectResult> GetUserData()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var user = await accountService.FindByIdAsync(userId);
                return new OkObjectResult(user);
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}