using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static LevelCounter.Models.ApplicationUser;

namespace LevelCounter.Models
{
    public class InGameUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
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
        public int GameId { get; set; }
        public Game Game { get; set; }
        public string Sex
        {
            get
            {
                switch (Gender)
                {
                    case (Genders.MALE):
                        return "male";
                    case (Genders.FEMALE):
                        return "female";
                }
                return "female"; ;
            }
            set
            {
                var word = value.ToLower();
                switch (word)
                {
                    case ("female"):
                        Gender = Genders.FEMALE;
                        break;
                    case ("male"):
                        Gender = Genders.MALE;
                        break;
                    default:
                        Gender = Genders.MALE;
                        break;
                }
            }
        }
        public Genders Gender { get; set; }
    }
}
