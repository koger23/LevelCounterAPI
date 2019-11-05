using System.Collections.Generic;

namespace LevelCounter.Models.DTO
{
    public class NewGameRequest
    {
        public int GameId { get; set; }
        public List<string> UserNames { get; set; }
    }
}
