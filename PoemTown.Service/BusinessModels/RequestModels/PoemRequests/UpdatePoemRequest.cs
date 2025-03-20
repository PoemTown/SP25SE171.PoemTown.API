using PoemTown.Repository.Enums.Poems;
using PoemTown.Service.BusinessModels.RequestModels.RecordFileRequests;

namespace PoemTown.Service.BusinessModels.RequestModels.PoemRequests;

public class UpdatePoemRequest
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    
    public string? Content { get; set; }
    
    public string? Description { get; set; }
    
    public int? ChapterNumber { get; set; }
    
    public string? ChapterName { get; set; }
    
    public PoemStatus? Status { get; set; }
    public Guid? CollectionId { get; set; }
    
    public Guid? SourceCopyRightId { get; set; }
    
    public string? PoemImage { get; set; }
    
    public ICollection<CreateNewRecordFileRequest>? RecordFiles { get; set; }

    public PoemType? Type { get; set; }
    public bool? IsSellCopyRight { get; set; }
}