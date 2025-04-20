using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class SystemContact : BaseEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
}