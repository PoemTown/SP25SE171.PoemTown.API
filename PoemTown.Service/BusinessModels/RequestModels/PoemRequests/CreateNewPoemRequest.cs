using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Service.BusinessModels.RequestModels.RecordFileRequests;

namespace PoemTown.Service.BusinessModels.RequestModels.PoemRequests;

public class CreateNewPoemRequest
{
    public string? Title { get; set; } = "";
    
    public string? Content { get; set; } = "";
    
    public string? Description { get; set; } = "";
    
    public int? ChapterNumber { get; set; }
    
    public string? ChapterName { get; set; } = "";
    
    public PoemStatus? Status { get; set; } = PoemStatus.Draft;
    public Guid? CollectionId { get; set; }
    
    public Guid? SourceCopyRightId { get; set; }
    
    public string? PoemImageUrl { get; set; } = null;
    
    public ICollection<CreateNewRecordFileRequest>? RecordFiles { get; set; }

    public PoemType? Type { get; set; }
}