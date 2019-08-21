using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HotelBookingApp.Models.DTO;
using LevelCounter.Exceptions;
using LevelCounter.Models;
using LevelCounter.Models.DTO;
using LevelCounter.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LevelCounter.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private const string DEFAULT_ROLE = "User";
        private readonly string apiSecretKey;

        public AccountService(ApplicationContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            apiSecretKey = configuration.GetSection("APISecretKey").Value;
        }
        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return user;
            }
            throw new ItemNotFoundException();
        }

        public async Task<LoginResponse> SignInAsync(LoginRequest request)
        {
            var errors = new List<string>();
            var result = await signInManager.PasswordSignInAsync(request.UserName, request.Password, request.RememberMe, lockoutOnFailure: false);
            checkLoginErrors(result, errors);
            var response = new LoginResponse
            {
                ErrorMessages = errors
            };

            if (errors.Count == 0)
            {
                var user = await userManager.FindByNameAsync    (request.UserName);
                response.Token = await GenerateJwtToken(user);
            }
            return response;
        }

        public List<string> checkLoginErrors(SignInResult result, List<string> errors)
        {
            if (result.IsLockedOut)
            {
                errors.Add("User account locked out.");
            }
            if (result.IsNotAllowed)
            {
                errors.Add("User is not allowed to login.");
            }
            if (result.RequiresTwoFactor)
            {
                errors.Add("Two factor authentication is required.");
            }
            if (!result.Succeeded)
            {
                errors.Add("Invalid login attempt");
            }
            return errors;
        }

        public Task SignOutAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<string>> SignUpAsync(SignupRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                Statistics = new Statistics()
            };
            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, DEFAULT_ROLE);
                await signInManager.SignInAsync(user, isPersistent: false);
            }
            return result.Errors
                .Select(e => e.Description)
                .ToList();
        }

        public async Task<List<string>> UpdateAsync(string userId, UserEditRequest userEditRequest)
        {
            var errorList = new List<string>();
            var user = await userManager.FindByIdAsync(userId);
            user.Email = userEditRequest.Email;
            user.UserName = userEditRequest.UserName;
            var passwordChangeResult = await userManager.ChangePasswordAsync(user, userEditRequest.CurrentPassword, userEditRequest.NewPassword);
            if (!passwordChangeResult.Succeeded)
            {
                errorList.AddRange(passwordChangeResult.Errors
                    .Select(e => e.Description)
                    .ToList());
            }
            var userUpdateResult = await userManager.UpdateAsync(user);
            if (!userUpdateResult.Succeeded)
            {
                errorList.AddRange(userUpdateResult.Errors
                                    .Select(e => e.Description)
                                    .ToList());
            }
            return errorList;
        }

        public async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, userRoles[0])
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMonths(1);

            var token = new JwtSecurityToken(
                "http://www.kogero.com",
                "http://www.kogero.com",
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
