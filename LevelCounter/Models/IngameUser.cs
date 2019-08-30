using System.ComponentModel.DataAnnotations;

namespace LevelCounter.Models
{
    public class InGameUser
    {
        [Key]
        public int InGameUserId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Level { get; set; }
        public int Bonus { get; set; }
        public int GameId { get; set; }
        public ApplicationUser.Genders Gender { get; set; } = ApplicationUser.Genders.MALE;
        public InGameUser()
        {
            Level = 1;
            Bonus = 0;
        }
    }
}
