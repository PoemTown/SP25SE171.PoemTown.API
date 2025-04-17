namespace PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;

public class GetPoemSampleResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public GetPoemResponse? Poem { get; set; }
}