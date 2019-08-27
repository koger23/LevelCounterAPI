using System;
using static LevelCounter.Models.ApplicationUser;

namespace LevelCounter.Models.DTO
{
    public class UserResponse : IUserDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Sex { get; set; }
        public Genders Gender { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public int StatisticsId { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsFriend { get; set; } = false;
        public bool IsBlocked { get; set; } = false;
        public int? RelationShipId { get; set; }
    }
}
