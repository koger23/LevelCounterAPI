using AutoMapper;
using LevelCounter.Models;
using LevelCounter.Models.DTO;
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
        private readonly IMapper mapper;

        public UserService(UserManager<ApplicationUser> userManager, ApplicationContext context, IMapper mapper)
        {
            this.userManager = userManager;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<UserShortResponse>> GetFriendsAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var relationships = context.Relationships
                .Where(r => r.RelationshipState == Relationship.RelationshipStates.CONFIRMED)
                .Where(r => r.RelatingUserId == user.Id || r.UserId == user.Id)
                .ToList();
            return GetUserShortResponseFromRelationshipList(relationships, userId);
        }

        private List<UserShortResponse> GetUserShortResponseFromRelationshipList(List<Relationship> relationships, string userId)
        {
            var shortResponses = new List<UserShortResponse>();
            for (int i = 0; i < relationships.Count; i++)
            {
                if (relationships[i].RelatingUserId == userId)
                {
                    shortResponses.Add(FindUserById(relationships[i].UserId));
                } else
                {
                    shortResponses.Add(FindUserById(relationships[i].RelatingUserId));
                }
            }
            return shortResponses;
        }

        public UserShortResponse FindUserById(string userId)
        {
            return mapper.Map<UserShortResponse>(context.Users.Where(u => u.Id == userId).SingleOrDefault());
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
