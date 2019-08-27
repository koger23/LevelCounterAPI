using AutoMapper;
using LevelCounter.Models;
using LevelCounter.Models.DTO;
using LevelCounter.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRelationshipService relationshipService;

        public UserService(UserManager<ApplicationUser> userManager, ApplicationContext context, IMapper mapper, IRelationshipService relationshipService)
        {
            this.userManager = userManager;
            this.context = context;
            this.mapper = mapper;
            this.relationshipService = relationshipService;
        }

        public List<UserShortResponse> GetFriendsAsync(string userId)
        {
            var relationships = context.Relationships
                .Where(r => r.RelationshipState == Relationship.RelationshipStates.CONFIRMED)
                .Where(r => r.RelatingUserId == userId || r.UserId == userId)
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
                }
                else
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

        public List<Relationship> GetPendingRequests(string userId)
        {
            var requests = context.Relationships
                .Where(r => r.RelationshipState == Relationship.RelationshipStates.PENDING)
                .Where(r => r.RelatingUserId == userId)
                .ToList() ?? new List<Relationship>();
            return requests;
        }

        public List<Relationship> GetUnconfirmedAsync(string userId)
        {
            var requests = context.Relationships
                .Where(r => r.RelationshipState == Relationship.RelationshipStates.PENDING)
                .Where(r => r.UserId == userId)
                .ToList() ?? new List<Relationship>();
            return requests;
        }

        public List<Relationship> GetBlocked(string userId)
        {
            var requests = context.Relationships
                .Where(r => r.RelationshipState == Relationship.RelationshipStates.BLOCKED)
                .Where(r => r.UserId == userId)
                .ToList() ?? new List<Relationship>();
            return requests;
        }

        public async Task<List<IUserDTO>> GetUsersAsync(string userId)
        {
            var users = new List<IUserDTO>();
            await context.Users
                .Where(u => u.Id != userId)
                .Where(u => u.IsPublic == true)
                .Where(u => u.UserName != "admin")
                .ForEachAsync(u => users.Add(mapper.Map<UserResponse>(u)));
            await context.Users
                .Where(u => u.Id != userId)
                .Where(u => u.IsPublic == false)
                .Where(u => u.UserName != "admin")
                .ForEachAsync(u => users.Add(mapper.Map<UserShortResponse>(u)));
            foreach (IUserDTO userDto in users)
            {
                var relationship = relationshipService.GetRelationshipByNames(userDto.UserName, userId);
                if (relationship != null)
                {
                    if (relationship.RelationshipState == Relationship.RelationshipStates.CONFIRMED)
                    {
                        userDto.IsFriend = true;
                        userDto.IsBlocked = false;
                        userDto.RelationShipId = relationship.RelationshipId;
                    }
                    else if (relationship.RelationshipState == Relationship.RelationshipStates.BLOCKED)
                    {
                        userDto.IsFriend = false;
                        userDto.IsBlocked = true;
                        userDto.RelationShipId = relationship.RelationshipId;
                    } else
                    {
                        userDto.IsFriend = false;
                        userDto.IsBlocked = false;
                        userDto.RelationShipId = null;
                    }
                }
            }
            return users;
        }
    }
}
