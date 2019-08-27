using HotelBookingApp.Models.DTO;
using LevelCounter.Models;
using LevelCounter.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public interface IAccountService
    {
        Task<LoginResponse> SignInAsync(LoginRequest request);
        Task<SignUpResponse> SignUpAsync(SignupRequest request);
        Task<UserResponse> FindByIdAsync(string userId);
        Task<List<string>> UpdateAsync(string userId, UserEditRequest userEditRequest);
        Task<int> GetUserStatisticIdAsync(string userId);
        Task<ApplicationUser> FindUserByIdAsync(string userId);
        ApplicationUser FindUserByName(string userId);
    }
}
