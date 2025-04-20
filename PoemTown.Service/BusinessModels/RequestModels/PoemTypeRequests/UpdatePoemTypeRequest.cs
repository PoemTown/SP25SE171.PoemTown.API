namespace PoemTown.Service.BusinessModels.RequestModels.PoemTypeRequests;

public class UpdatePoemTypeRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? GuideLine { get; set; }
    public string? Color { get; set; }
}