using LevelCounter.Models;
using LevelCounter.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public interface IUserService
    {
        Task<List<UserShortResponse>> GetFriendsAsync(string userId);
        Task<List<Relationship>> GetPendingRequestsAsync(string userId);
        Task<List<Relationship>> GetUnconfirmedAsync(string userId);
        Task<List<Relationship>> GetBlockedAsync(string userId);
        UserShortResponse FindUserById(string userId);
    }
}