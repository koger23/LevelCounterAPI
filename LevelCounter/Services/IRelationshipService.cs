using LevelCounter.Models;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public interface IRelationshipService
    {
        Task<Relationship> MakeFriendRequest(string firendName, string userId);
        Task ConfirmRequest(int relationshipId, string userId);
        Task BlockRequest(int relationshipId);
        Relationship GetRelationshipByNames(string friendName, string userId);
        Task BlockUser(string userName, string userId);
        Task DismissRequest(int relationshipId);
    }
}
