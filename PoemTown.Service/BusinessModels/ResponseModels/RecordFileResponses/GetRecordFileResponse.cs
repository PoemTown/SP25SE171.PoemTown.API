namespace PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;

public class GetRecordFileResponse
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public bool IsPublic { get; set; }
}