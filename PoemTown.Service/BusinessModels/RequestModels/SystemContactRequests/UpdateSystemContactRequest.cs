namespace PoemTown.Service.BusinessModels.RequestModels.SystemContactRequests;

public class UpdateSystemContactRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = String.Empty;
    public string? Icon { get; set; } = String.Empty;
    public string? Description { get; set; } = String.Empty;
}