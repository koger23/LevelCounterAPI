using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Models.DTO;
using LevelCounter.Exceptions;
using LevelCounter.Models;
using LevelCounter.Models.DTO;
using LevelCounter.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace LevelCounter.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private const string DEFAULT_ROLE = "User";

        public AccountService(ApplicationContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
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

        public Task<List<string>> SignInAsync(LoginRequest request)
        {
            throw new System.NotImplementedException();
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
    }
}
