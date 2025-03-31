namespace PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

public class GetBasicUserInformationResponse
{
    public Guid Id { get; set; }
    public string? DisplayName { get; set; }
    public string? UserName { get; set; }
    public string? Avatar { get; set; }
}