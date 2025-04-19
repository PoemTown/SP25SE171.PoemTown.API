namespace PoemTown.Service.BusinessModels.RequestModels.PoetSampleRequests;

public class CreateNewPoetSampleRequest
{
    public string Name { get; set; }
    public string? Bio { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Avatar { get; set; }
    public IList<Guid>? TitleSampleIds { get; set; }
}