using Newtonsoft.Json;

namespace LevelCounter.Models
{
    public class Statistics
    {
        public int StatisticsId { get; set; }
        public int Wins { get; set; }
        public int GamesPlayed { get; set; }
        public int RoundsPlayed { get; set; }
        public long PlayTime { get; set; }
        [JsonIgnore]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
