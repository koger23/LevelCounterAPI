using System.Collections.Generic;

namespace LevelCounter.Models.DTO
{
    public class GameViewModel
    {
        public int GameId { get; set; }
        public long Time { get; set; }
        public List<InGameUser> InGameUsers { get; set; }
        public string HostingUserId { get; set; }
    }
}
