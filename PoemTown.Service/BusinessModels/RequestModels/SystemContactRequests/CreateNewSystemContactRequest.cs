namespace PoemTown.Service.BusinessModels.RequestModels.SystemContactRequests;

public class CreateNewSystemContactRequest
{
    public string Name { get; set; } = String.Empty;
    public string? Icon { get; set; } = String.Empty;
    public string? Description { get; set; } = String.Empty;
}