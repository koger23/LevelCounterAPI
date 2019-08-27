namespace LevelCounter.Models.DTO
{
    public class UserShortResponse : IUserDTO
    {
        public string UserName { get; set; }
        public int StatisticsId { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsFriend { get; set; } = false;
        public bool IsBlocked { get; set; } = false;
        public int? RelationShipId { get; set; }
    }
}
