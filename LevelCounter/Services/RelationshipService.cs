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
            if (existingRelationship != null)
            {
                existingRelationship.RelationshipState = Relationship.RelationshipStates.PENDING;
                await UpdateAndSaveAsync(existingRelationship);
                return existingRelationship;
            }
            return await CreateNewRelationship(user, friend);
        }

        public async Task ConfirmRequest(int relationshipId, string userId)
        {
            var relationship = GetRelationshipById(relationshipId);
            if (relationship.RelatingUserId == userId)
            {
                relationship.RelationshipState = Relationship.RelationshipStates.CONFIRMED;
                await UpdateAndSaveAsync(relationship);
            }
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

        public Relationship GetRelationshipByNames(string friendName, string userId)
        {
            var relation1 = context.Relationships
                .Where(r => r.UserId == userId)
                .Where(r => r.RelatingUser.UserName == friendName)
                .SingleOrDefault();
            var relation2 = context.Relationships
                .Where(r => r.RelatingUserId == userId)
                .Where(r => r.User.UserName == friendName)
                .SingleOrDefault();
            return relation1 ?? relation2;
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

        public async Task BlockUser(string userName, string userId)
        {
            var relationship = GetRelationshipByNames(userName, userId) ?? throw new ItemNotFoundException();
            relationship.RelationshipState = Relationship.RelationshipStates.BLOCKED;
            context.Relationships.Update(relationship);
            await context.SaveChangesAsync();
        }
    }
}
