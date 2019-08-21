using LevelCounter.Models;
using LevelCounter.Repository;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationContext context;

        public UserService(UserManager<ApplicationUser> userManager, ApplicationContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public async Task<List<Relationship>> GetFriendsAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var friends = context.Relationships
                .Where(r => r.RelationshipState == Relationship.RelationshipStates.CONFIRMED)
                .Where(r => r.RelatingUserId == user.Id || r.UserId == user.Id)
                .ToList();
            return friends;
        }

        public async Task<List<Relationship>> GetPendingRequestsAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var requests = context.Relationships
                .Where(r => r.RelationshipState == Relationship.RelationshipStates.PENDING)
                .Where(r => r.RelatingUserId == user.Id)
                .ToList() ?? new List<Relationship>();
            return requests;
        }

        public async Task<List<Relationship>> GetUnconfirmedAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var requests = context.Relationships
                .Where(r => r.RelationshipState == Relationship.RelationshipStates.PENDING)
                .Where(r => r.UserId == user.Id)
                .ToList() ?? new List<Relationship>();
            return requests;
        }

        public async Task<List<Relationship>> GetBlockedAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var requests = context.Relationships
                .Where(r => r.RelationshipState == Relationship.RelationshipStates.BLOCKED)
                .Where(r => r.UserId == user.Id)
                .ToList() ?? new List<Relationship>();
            return requests;
        }
    }
}
