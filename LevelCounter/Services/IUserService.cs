using LevelCounter.Models;
using LevelCounter.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public interface IUserService
    {
        List<UserShortResponse> GetFriendsAsync(string userId);
        List<Relationship> GetPendingRequests(string userId);
        List<Relationship> GetUnconfirmedAsync(string userId);
        List<Relationship> GetBlocked(string userId);
        UserShortResponse FindUserById(string userId);
        Task<List<IUserDTO>> GetUsersAsync(string userId);
    }
}