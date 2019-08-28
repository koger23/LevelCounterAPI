using AutoMapper;
using LevelCounter.Models;
using LevelCounter.Models.DTO;
using LevelCounter.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LevelCounter.Models.Relationship;

namespace LevelCounter.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext context;
        private readonly IMapper mapper;
        private readonly IRelationshipService relationshipService;

        public UserService(ApplicationContext context, IMapper mapper, IRelationshipService relationshipService)
        {
            this.context = context;
            this.mapper = mapper;
            this.relationshipService = relationshipService;
        }

        public List<IUserDTO> GetFriendsAsync(string userId)
        {
            var relationships = context.Relationships
                .Where(r => r.RelationshipState == Relationship.RelationshipStates.CONFIRMED)
                .Where(r => r.RelatingUserId == userId || r.UserId == userId)
                .ToList();
            return GetUserShortResponseFromRelationshipList(relationships, userId);
        }

        private List<IUserDTO> GetUserShortResponseFromRelationshipList(List<Relationship> relationships, string userId)
        {
            var shortResponses = new List<IUserDTO>();
            for (int i = 0; i < relationships.Count; i++)
            {   
                var user = new UserShortResponse();
                var relationship = relationships[i];
                if (relationship.RelatingUserId == userId)
                {
                    var relatedUserId = relationships[i].UserId;
                    user = FindUserById(relatedUserId);
                }
                else
                {
                    var relatinUserId = relationships[i].RelatingUserId;
                    user = FindUserById(relatinUserId);
                }
                SetBoolsBasedOnRelationshipState(relationship, user);
                shortResponses.Add(user);
                
            }
            return shortResponses;
        }

        private IUserDTO SetBoolsBasedOnRelationshipState(Relationship relationship, IUserDTO user)
        {
            var state = relationship.RelationshipState;
            switch (state)
            {
                case (RelationshipStates.BLOCKED):
                    user.IsBlocked = true;
                    user.IsPending = false;
                    user.IsFriend = false;
                    break;
                case (RelationshipStates.CONFIRMED):
                    user.IsBlocked = false;
                    user.IsPending = false;
                    user.IsFriend = true;
                    break;
                case (RelationshipStates.PENDING):
                    user.IsBlocked = false;
                    user.IsPending = true;
                    user.IsFriend = false;
                    break;
                case (RelationshipStates.UNKNOWN):
                    user.IsBlocked = false;
                    user.IsPending = false;
                    user.IsFriend = false;
                    break;
            }
            return user;
        }

        public UserShortResponse FindUserById(string userId)
        {
            return mapper.Map<UserShortResponse>(context.Users.Where(u => u.Id == userId).SingleOrDefault());
        }

        public List<UserShortResponse> GetPendingRequests(string userId)
        {
            var userShortResponses = new List<UserShortResponse>();
            var requests = context.Relationships
                .Where(r => r.RelatingUserId == userId)
                .Where(r => r.RelationshipState == Relationship.RelationshipStates.PENDING)
                .ToList() ?? new List<Relationship>();
             requests.ForEach(r =>
            {
                var user = context.Users
                .Where(u => u.Id == r.UserId)
                .SingleOrDefault();
                var userResponse = mapper.Map<UserShortResponse>(user);
                userResponse.RelationShipId = r.RelationshipId;
                userResponse.IsPending = true;
                userShortResponses.Add(userResponse);
            });
            return userShortResponses;
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
                    }
                    else if (relationship.RelationshipState == Relationship.RelationshipStates.PENDING)
                    {
                        userDto.IsFriend = false;
                        userDto.IsBlocked = false;
                        userDto.IsPending = true;
                        userDto.RelationShipId = relationship.RelationshipId;
                    } else
                    {
                        userDto.IsFriend = false;
                        userDto.IsBlocked = false;
                        userDto.IsPending = false;
                        userDto.RelationShipId = null;
                    }
                }
            }
            return users;
        }
    }
}
