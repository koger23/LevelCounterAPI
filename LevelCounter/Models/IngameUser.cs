namespace LevelCounter.Models
{
    public class InGameUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Level
        {
            get
            {
                return Level;
            }
            set
            {
                Level = value <= 1 ? 1 : value;
            }
        }
        public int Bonus
        {
            get
            {
                return Bonus;
            }
            set
            {
                Bonus = value <= 0 ? 0 : value;
            }
        }
        public int Strength
        {
            get
            {
                return Level + Bonus;
            }
        }

        public InGameUser()
        {
            Bonus = 0;
            Level = 1;
        }
    }
}
