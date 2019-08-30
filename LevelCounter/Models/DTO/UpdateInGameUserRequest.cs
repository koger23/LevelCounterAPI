using System.ComponentModel.DataAnnotations.Schema;

namespace LevelCounter.Models.DTO
{
    [NotMapped]
    public class UpdateInGameUserRequest
    {
        public int InGameUserId { get; set; }
        public int GameId { get; set; }
        public int Level { get; set; }
        public int Bonus { get; set; }
    }
}
