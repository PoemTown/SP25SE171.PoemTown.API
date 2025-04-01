using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;

public class GetRecordFileResponse : BaseEntity
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public bool IsPublic { get; set; }
    public GetBasicUserInformationResponse Owner { get; set; }  
    public GetPoemDetailResponse Poem {  get; set; }    
}