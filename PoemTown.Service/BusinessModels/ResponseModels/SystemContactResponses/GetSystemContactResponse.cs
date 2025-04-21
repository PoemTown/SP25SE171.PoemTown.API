namespace PoemTown.Service.BusinessModels.ResponseModels.SystemContactResponses;

public class GetSystemContactResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Icon { get; set; }
    public string? Description { get; set; }
}