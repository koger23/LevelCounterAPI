namespace LevelCounter.Models.DTO
{
    public interface IUserDTO
    {
        string UserName { get; set; }
        int StatisticsId { get; set; }
        string AvatarUrl { get; set; }
        int? RelationShipId { get; set; }
        bool IsFriend { get; set; }
        bool IsBlocked { get;set; }
    }
}
