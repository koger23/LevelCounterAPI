using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LevelCounter.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        [NotMapped]
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
        public List<Relationship> Relationships { get; set; }
        public bool IsPublic { get; set; } = true;
    }
}
