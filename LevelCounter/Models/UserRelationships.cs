namespace LevelCounter.Models
{
    public class UserRelationships
    {
        public int id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int RelationshipId { get; set; }
        public Relationship Relationship { get; set; }
    }
}
