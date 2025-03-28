using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;

public class GetRecordFileResponse : BaseEntity
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public bool IsPublic { get; set; }
    public GetPoemDetailResponse Poem {  get; set; }    
}