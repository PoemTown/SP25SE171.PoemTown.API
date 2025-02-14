namespace PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

public class GetBasicAuthorInformationResponse
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
}