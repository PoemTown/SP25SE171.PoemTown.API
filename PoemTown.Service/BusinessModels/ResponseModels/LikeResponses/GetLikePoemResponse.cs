using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.LikeResponses;

public class GetLikePoemResponse
{
    public int TotalLikes { get; set; }
    public List<GetBasicUserInformationResponse>? User { get; set; }
}