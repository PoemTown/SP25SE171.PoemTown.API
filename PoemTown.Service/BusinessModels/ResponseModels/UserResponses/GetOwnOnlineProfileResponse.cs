namespace PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

public class GetOwnOnlineProfileResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
    public int TotalFollowers { get; set; }
    public int TotalFollowings { get; set; }
}