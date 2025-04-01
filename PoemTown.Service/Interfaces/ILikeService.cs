using PoemTown.Service.BusinessModels.ResponseModels.LikeResponses;

namespace PoemTown.Service.Interfaces;

public interface ILikeService
{
    Task LikePoem(Guid userId, Guid poemId);
    Task DislikePoem(Guid userId, Guid poemId);
    Task<GetLikePoemResponse> GetLikePoem(Guid poemId);
}