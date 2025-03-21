using PoemTown.Repository.Base;
using PoemTown.Repository.Enums;

namespace PoemTown.Repository.Entities;

public class Achievement : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public AchievementType Type { get; set; }
    public int Rank { get; set; }
    public DateTimeOffset EarnedDate { get; set; }
    public Guid UserId { get; set; }
    public virtual User? User { get; set; }
}