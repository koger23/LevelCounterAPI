using LevelCounter.Exceptions;
using LevelCounter.Models;
using LevelCounter.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public class RelationshipService : IRelationshipService
    {
        private readonly ApplicationContext context;
        private readonly IAccountService accountService;

        public RelationshipService(ApplicationContext context, IAccountService accountService)
        {
            this.context = context;
            this.accountService = accountService;
        }

        public async Task<Relationship> MakeFriendRequest(string friendName, string userId)
        {
            var user = await accountService.FindUserByIdAsync(userId);
            var friend = accountService.FindUserByName(friendName);
            var existingRelationship = GetRelationshipByNames(friendName, userId);
            if (CheckRelationshipCanBePending(existingRelationship))
            {
                existingRelationship.RelationshipState = Relationship.RelationshipStates.PENDING;
                await UpdateAndSaveAsync(existingRelationship);
                return existingRelationship;
            }
            return await CreateNewRelationship(user, friend);
        }

        public async Task ConfirmRequest(int relationshipId)
        {
            var relationship = GetRelationshipById(relationshipId);
            relationship.RelationshipState = Relationship.RelationshipStates.CONFIRMED;
            await UpdateAndSaveAsync(relationship);
        }

        public async Task BlockRequest(int relationshipId)
        {
            var relationship = GetRelationshipById(relationshipId);
            relationship.RelationshipState = Relationship.RelationshipStates.BLOCKED;
            await UpdateAndSaveAsync(relationship);
        }

        public async Task DismissRequest(int relationshipId)
        {
            var relationship = GetRelationshipById(relationshipId);
            relationship.RelationshipState = Relationship.RelationshipStates.UNKNOWN;
            await UpdateAndSaveAsync(relationship);
        }

        private bool CheckRelationshipCanBePending(Relationship relationship)
        {
            if (relationship != null && relationship.RelationshipState != Relationship.RelationshipStates.BLOCKED)
            {
                return true;
            }
            return false;
        }

        private async Task<Relationship> CreateNewRelationship(ApplicationUser requester, ApplicationUser friend)
        {
            var relationship = new Relationship
            {
                User = requester,
                RelatingUser = friend,
                RelationshipState = Relationship.RelationshipStates.PENDING
            };
            await context.Relationships.AddAsync(relationship);
            await context.SaveChangesAsync();
            return relationship;
        }

        private Relationship GetRelationshipByNames(string friendName, string userId)
        {
            return context.Relationships
                .Where(r => r.UserId == userId)
                .Where(r => r.RelatingUser.UserName == friendName)
                .SingleOrDefault();
        }

        private async Task UpdateAndSaveAsync(Relationship relationship)
        {
            context.Update(relationship);
            await context.SaveChangesAsync();
        }

        private Relationship GetRelationshipById(int relationshipId)
        {
            return context.Relationships
                .Where(r => r.RelationshipId == relationshipId)
                .SingleOrDefault() ?? throw new ItemNotFoundException();
        }
    }
}
