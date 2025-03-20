using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class LoginTracking : BaseEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset LoginDate { get; set; }

    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}