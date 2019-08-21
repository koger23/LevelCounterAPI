using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LevelCounter.Models.ApplicationUser;

namespace LevelCounter.Models.DTO
{
    public class UserResponse
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public int StatisticsId { get; set; }
        public string AvatarUrl { get; set; }
    }
}
