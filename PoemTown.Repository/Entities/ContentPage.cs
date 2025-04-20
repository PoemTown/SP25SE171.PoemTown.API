using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class ContentPage : BaseEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Content { get; set; } = String.Empty;
}