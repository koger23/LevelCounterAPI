using LevelCounter.Models;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public interface IRelationshipService
    {
        Task<Relationship> MakeFriendRequest(string firendName, string userId);
        Task ConfirmRequest(int relationshipId);
        Task BlockRequest(int relationshipId);
    }
}
