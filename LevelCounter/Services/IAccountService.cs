using HotelBookingApp.Models.DTO;
using LevelCounter.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public interface IAccountService
    {
        Task<LoginResponse> SignInAsync(LoginRequest request);
        Task SignOutAsync();
        Task<List<string>> SignUpAsync(SignupRequest request);
        Task<UserResponse> FindByIdAsync(string userId);
        Task<List<string>> UpdateAsync(string userId, UserEditRequest userEditRequest);
        Task<int> GetUserStatisticId(string userId);
    }
}
