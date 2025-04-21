namespace PoemTown.Service.BusinessModels.RequestModels.ContentPageRequests;

public class UpdateContentPageRequest
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
}