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
            var friend = await Task.Run(() =>
            {
                return context.Users.FirstOrDefault(u => u.UserName == friendName);
            });
            var existingRelationship = context.Relationships
                .Where(r => r.UserId == userId)
                .Where(r => r.RelatingUser.UserName == friendName)
                .SingleOrDefault();
            if (existingRelationship != null)
            {
                return existingRelationship;
            }

            var relationship = new Relationship
            {
                User = user,
                RelatingUser = friend,
                RelationshipState = Relationship.RelationshipStates.PENDING
            };
            await context.Relationships.AddAsync(relationship);
            await context.SaveChangesAsync();
            return relationship;
        }
    }
}
