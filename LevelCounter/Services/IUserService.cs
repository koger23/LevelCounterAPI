using LevelCounter.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public interface IUserService
    {
        Task<List<Relationship>> GetFriendsAsync(string userId);
        Task<List<Relationship>> GetPendingRequestsAsync(string userId);
        Task<List<Relationship>> GetUnconfirmedAsync(string userId);
        Task<List<Relationship>> GetBlockedAsync(string userId);
    }
}