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
        public Gender Sex { get; set; }
        public enum Gender
        {
            MALE,
            FEMALE
        }
        public int StatisticsId { get; set; }
        public Statistics Statistics { get; set; }
        [Required]
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public string AvatarUrl;
    }
}
