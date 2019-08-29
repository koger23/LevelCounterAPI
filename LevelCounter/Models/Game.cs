using Newtonsoft.Json;
using System.Collections.Generic;

namespace LevelCounter.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public long Time { get; set; }
        public List<InGameUser> InGameUsers { get; set; }
        public string ApplicationUserId { get; set; }
        [JsonIgnore]
        public ApplicationUser ApplicationUser { get; set; }
        public bool IsHosted { get; set; } = false;
    }
}
