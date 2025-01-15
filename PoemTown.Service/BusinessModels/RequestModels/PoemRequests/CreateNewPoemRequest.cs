using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums;

namespace PoemTown.Service.BusinessModels.RequestModels.PoemRequests;

public class CreateNewPoemRequest
{
    [Required]
    [FromForm(Name = "title")]
    public string Title { get; set; }
    
    [FromForm(Name = "content")]
    public string? Content { get; set; }
    
    [FromForm(Name = "description")]
    public string? Description { get; set; }
    
    [FromForm(Name = "chapterNumber")]
    public int? ChapterNumber { get; set; }
    
    [FromForm(Name = "chapterName")]
    public string? ChapterName { get; set; }
    
    [FromForm(Name = "poemImage")]
    public IFormFile? PoemImage { get; set; }
    
    [FromForm(Name = "collectionId")]
    public Guid? CollectionId { get; set; }
    
    [FromForm(Name = "sourceCopyRight")]
    public Guid? SourceCopyRight { get; set; } 
    
    [FromForm(Name = "recordFiles")]
    public IList<IFormFile>? RecordFiles { get; set; }
    
    [FromForm(Name = "isDraft")]
    public bool? IsDraft { get; set; }
    
    [FromForm(Name = "type")]
    public PoemType Type { get; set; }
}