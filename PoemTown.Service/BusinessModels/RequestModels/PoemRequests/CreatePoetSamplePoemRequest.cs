using PoemTown.Repository.Enums.Poems;

namespace PoemTown.Service.BusinessModels.RequestModels.PoemRequests;

public class CreatePoetSamplePoemRequest
{
    public string? Title { get; set; } = "";
    
    public string? Content { get; set; } = "";
    
    public string? Description { get; set; } = "";
    public PoemStatus? Status { get; set; } = PoemStatus.Draft;
    public string? PoemImage { get; set; } = null;
    public required Guid? PoemTypeId { get; set; }
    public Guid? CollectionId { get; set; }
}