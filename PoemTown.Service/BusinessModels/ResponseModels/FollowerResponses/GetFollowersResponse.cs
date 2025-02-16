using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.FollowerResponses;

public class GetFollowersResponse
{
    public Guid Id { get; set; }
    public GetBasicUserInformationResponse User { get; set; }
}