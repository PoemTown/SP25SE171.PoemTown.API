namespace PoemTown.Service.BusinessModels.ResponseModels.PoemTypeResponses;

public class GetPoemTypeResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? GuideLine { get; set; }
    public string? Color { get; set; }
}