﻿using HotelBookingApp.Models.DTO;
using LevelCounter.Models;
using LevelCounter.Models.DTO;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public interface IAccountService
    {
        Task<List<string>> SignInAsync(LoginRequest request);
        Task SignOutAsync();
        Task<List<string>> SignUpAsync(SignupRequest request);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<ApplicationUser> FindByIdAsync(string userId);
    }
}