namespace PoemTown.Repository.Entities;

public class Achievement
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime EarnedDate { get; set; }
    public Guid UserId { get; set; }
    public virtual User? User { get; set; }
}