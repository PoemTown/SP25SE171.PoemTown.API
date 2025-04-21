using PoemTown.Repository.Enums.Poems;

namespace PoemTown.Service.BusinessModels.RequestModels.PoemRequests;

public class UpdatePoetSamplePoemRequest
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Description { get; set; }
    public PoemStatus? Status { get; set; }
    public Guid? CollectionId { get; set; }
    public string? PoemImage { get; set; }
    public required Guid PoemTypeId { get; set; }
}