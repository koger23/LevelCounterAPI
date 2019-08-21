using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace LevelCounter.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        [Required]
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
                return "Invalid gender type";
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
        public enum Genders
        {
            MALE = 1,
            FEMALE = 0
        }
        public int StatisticsId { get; set; }
        public Statistics Statistics { get; set; }
        [Required]
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public string AvatarUrl;
    }
}
