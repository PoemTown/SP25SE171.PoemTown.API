using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.CommentRequests;
using PoemTown.Service.BusinessModels.ResponseModels.CommentResponses;
using PoemTown.Service.QueryOptions.FilterOptions.CommentFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.CommentSorts;

namespace PoemTown.Service.Interfaces;

public interface ICommentService
{
    Task CommentPoem(Guid userId, CommentPoemRequest request);
    Task ReplyComment(Guid userId, ReplyCommentRequest request);
    Task DeleteCommentPermanent(Guid userId, Guid commentId);

    Task<PaginationResponse<GetCommentResponse>>
        GetPostComments(Guid poemId, RequestOptionsBase<GetPostCommentFilterOption, GetPostCommentSortOption> request);
}