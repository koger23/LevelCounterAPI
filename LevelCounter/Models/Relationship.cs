using Newtonsoft.Json;
using System.Collections.Generic;

namespace LevelCounter.Models
{
    public class Relationship
    {
        public int RelationshipId { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        public string RelatingUserId { get; set; }
        [JsonIgnore]
        public ApplicationUser RelatingUser { get; set; }
        public string State
        {
            get
            {
                switch (RelationshipState)
                {
                    case (RelationshipStates.BLOCKED):
                        return "blocked";
                    case (RelationshipStates.CONFIRMED):
                        return "confirmed";
                    case (RelationshipStates.PENDING):
                        return "pending";
                    case (RelationshipStates.UNKNOWN):
                        return "unknown";
                }
                return "Invalid gender type";
            }
            set
            {
                var word = value.ToLower();
                switch (word)
                {
                    case ("blocked"):
                        RelationshipState = RelationshipStates.BLOCKED;
                        break;
                    case ("confirmed"):
                        RelationshipState = RelationshipStates.CONFIRMED;
                        break;
                    case ("pending"):
                        RelationshipState = RelationshipStates.PENDING;
                        break;
                    case ("unkown"):
                        RelationshipState = RelationshipStates.UNKNOWN;
                        break;
                }
            }
        }
        public RelationshipStates RelationshipState { get; set; } = RelationshipStates.UNKNOWN;
        public enum RelationshipStates
        {
            BLOCKED = 0,
            CONFIRMED = 1,
            PENDING = 2,
            UNKNOWN = 3
        }
    }
}
