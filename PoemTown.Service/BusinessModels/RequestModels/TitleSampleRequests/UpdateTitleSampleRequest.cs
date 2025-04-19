namespace PoemTown.Service.BusinessModels.RequestModels.TitleSampleRequests;

public class UpdateTitleSampleRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}