using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Announcements;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.CommentRequests;
using PoemTown.Service.BusinessModels.ResponseModels.CommentResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Events.AnnouncementEvents;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.CommentFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.CommentSorts;

namespace PoemTown.Service.Services;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public CommentService(IUnitOfWork unitOfWork, 
        IMapper mapper,
        IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task CommentPoem(Guid userId, CommentPoemRequest request)
    {
        // Check if poem exists
        Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == request.PoemId);
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }

        // Check if poem is posted, if not, throw exception
        if (poem.Status != PoemStatus.Posted)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Can not comment to unpublished poem");
        }
        
        Comment comment = new()
        {
            PoemId = request.PoemId,
            AuthorCommentId = userId,
            Content = request.Content,
            CreatedTime = DateTimeHelper.SystemTimeNow,
        };
        
        await _unitOfWork.GetRepository<Comment>().InsertAsync(comment);
        await _unitOfWork.SaveChangesAsync();
        
        // Get user information who like the poem
        User? user = await _unitOfWork.GetRepository<User>()
            .FindAsync(p => p.Id == userId);
        if (user != null && poem.UserId != userId)
        {
            // Get total likes for the poem
            var totalComments = await _unitOfWork.GetRepository<Comment>()
                .AsQueryable()
                .Where(p => p.PoemId == poem.Id)
                .CountAsync() - 1; // Exclude the current user who liked the poem
            
            // Announce to poem owner that their poem has been liked
            await _publishEndpoint.Publish(new UpdateAndSendUserAnnouncementEvent()
            {
                UserId = poem.UserId!.Value,
                Title = "Bài thơ của bạn có bình luận mới",
                Content = totalComments > 0 
                    ? $"Bài thơ: \"{poem.Title}\" của bạn đã được bình luận bởi {user.UserName} và {totalComments} người khác." 
                    : $"Bài thơ: \"{poem.Title}\" của bạn đã được bình luận bởi {user.UserName}",
                IsRead = false,
                Type = AnnouncementType.Comment
            });
        }
    }
    
    public async Task ReplyComment(Guid userId, ReplyCommentRequest request)
    {
        // Check if comment exists
        Comment? parentComment = await _unitOfWork.GetRepository<Comment>().FindAsync(c => c.Id == request.ParrentCommentId);
        if (parentComment == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Comment not found");
        }

        /*// Check if poem exists
        Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == parentComment.PoemId);
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }

        // Check if poem is posted, if not, throw exception
        if (poem.Status != PoemStatus.Posted)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Can not comment to unpublished poem");
        }*/

        Comment comment = new()
        {
            PoemId = parentComment.PoemId,
            AuthorCommentId = userId,
            Content = request.Content,
            ParentCommentId = request.ParrentCommentId,
            CreatedTime = DateTimeHelper.SystemTimeNow,
        };

        await _unitOfWork.GetRepository<Comment>().InsertAsync(comment);
        await _unitOfWork.SaveChangesAsync();
        
        User? user = await _unitOfWork.GetRepository<User>()
            .FindAsync(p => p.Id == userId);
        if (user != null && comment.Poem.UserId != userId)
        {
            // Get poem information
            var poem = await _unitOfWork.GetRepository<Poem>()
                .FindAsync(p => p.Id == parentComment.PoemId);

            if (poem == null)
            {
                return;
            }
            
            // Get total likes for the poem
            var totalComments = await _unitOfWork.GetRepository<Comment>()
                .AsQueryable()
                .Where(p => p.PoemId == poem.Id)
                .CountAsync() - 1; // Exclude the current user who liked the poem
            
            // Announce to poem owner that their poem has been liked
            await _publishEndpoint.Publish(new UpdateAndSendUserAnnouncementEvent()
            {
                UserId = poem.UserId!.Value,
                Title = "Bài thơ của bạn có bình luận mới",
                Content = totalComments > 0 
                    ? $"Bài thơ: \"{poem.Title}\" của bạn đã được bình luận bởi {user.UserName} và {totalComments} người khác." 
                    : $"Bài thơ: \"{poem.Title}\" của bạn đã được bình luận bởi {user.UserName}",
                IsRead = false,
                Type = AnnouncementType.Comment
            });
        }
    }
    
    public async Task DeleteCommentPermanent(Guid userId, Guid commentId)
    {
        Comment? comment = await _unitOfWork.GetRepository<Comment>().FindAsync(c => c.Id == commentId);
        if (comment == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Comment not found");
        }

        if (comment.AuthorCommentId != userId)
        {
            throw new CoreException(StatusCodes.Status403Forbidden, "You are not the author of this comment");
        }
        
        // Check if comment has children comments, if yes, remove parent comment id
        bool hasChildrenComments = await _unitOfWork.GetRepository<Comment>()
            .AsQueryable()
            .AnyAsync(c => c.ParentCommentId == commentId);
        if(hasChildrenComments)
        {
            var childrenComments = await _unitOfWork.GetRepository<Comment>()
                .AsQueryable()
                .Where(c => c.ParentCommentId == commentId)
                .ToListAsync();
            foreach (var children in childrenComments)
            {
                _unitOfWork.GetRepository<Comment>().DeletePermanent(children);
            }
        }

        _unitOfWork.GetRepository<Comment>().DeletePermanent(comment);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<PaginationResponse<GetCommentResponse>>
        GetPostComments(Guid? userId, Guid poemId, RequestOptionsBase<GetPostCommentFilterOption, GetPostCommentSortOption> request)
    {
        // Check if poem exists
        Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poemId);
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }

        var commentQuery = _unitOfWork.GetRepository<Comment>().AsQueryable();
        
        commentQuery = commentQuery.Where(c => c.PoemId == poemId);
        
        if (request.FilterOptions != null)
        {
        }

        commentQuery = request.SortOptions switch
        {
            GetPostCommentSortOption.CreatedTimeAscending => commentQuery.OrderBy(c => c.CreatedTime),
            GetPostCommentSortOption.CreatedTimeDescending => commentQuery.OrderByDescending(c => c.CreatedTime),
            _ => commentQuery.OrderByDescending(c => c.CreatedTime)
        };
        
        var queryPaging = await _unitOfWork.GetRepository<Comment>()
            .GetPagination(commentQuery, request.PageNumber, request.PageSize);

        IList<GetCommentResponse> comments = new List<GetCommentResponse>();
        foreach (var comment in queryPaging.Data)
        {
            Comment? commentEntity = await _unitOfWork.GetRepository<Comment>().FindAsync(p => p.Id == comment.Id);
            if(commentEntity == null)
            {
                continue;
            }
            
            comments.Add(_mapper.Map<GetCommentResponse>(commentEntity));
            
            // Map author information
            comments.Last().Author = _mapper.Map<GetBasicUserInformationResponse>(
                await _unitOfWork.GetRepository<User>().FindAsync(u => u.Id == commentEntity.AuthorCommentId));
            
            // Map is mine
            comments.Last().IsMine = commentEntity.AuthorCommentId == userId;
            
            // Map parent comment author information
            if (commentEntity.ParentCommentId != null)
            {
                // Get parentComment
                var parentComment = await _unitOfWork.GetRepository<Comment>()
                    .FindAsync(p => p.Id == commentEntity.ParentCommentId);
                
                if (parentComment == null)
                {
                    continue;    
                }
                comments.Last().ParentCommentAuthor = _mapper.Map<GetBasicUserInformationResponse>(
                    await _unitOfWork.GetRepository<User>().FindAsync(u => u.Id == parentComment.AuthorCommentId));
            }
        }
        
        return new PaginationResponse<GetCommentResponse>(comments, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }
}