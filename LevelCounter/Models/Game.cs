using System;
using System.Collections.Generic;

namespace LevelCounter.Models
{
    public class Game
    {
        public int Id { get; set; }
        public long Time { get; set; }
        public DateTime Datetime { get; set; } = DateTime.Now;
        public List<InGameUser> InGameUsers { get; set; } = new List<InGameUser>();
        public string HostingUserId { get; set; }
        public bool IsRunning { get; set; } = false;
        public int Rounds { get; set; } = 1;
    }
}
