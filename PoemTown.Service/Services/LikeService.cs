using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Announcements;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.ResponseModels.LikeResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Events.AnnouncementEvents;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Services;

public class LikeService : ILikeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public LikeService(IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task LikePoem(Guid userId, Guid poemId)
    {
        // Check if poem exists
        Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poemId);
        if(poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }
        
        var like = await _unitOfWork.GetRepository<Like>()
            .FindAsync(x => x.PoemId == poemId && x.UserId == userId);
        
        // If user already liked the poem, return
        if (like != null)
        {
            return;
        }

        like = new Like
        {
            PoemId = poemId,
            UserId = userId,
        };
        
        await _unitOfWork.GetRepository<Like>().InsertAsync(like);
        await _unitOfWork.SaveChangesAsync();
        
        // Get user information who like the poem
        User? user = await _unitOfWork.GetRepository<User>()
            .FindAsync(p => p.Id == userId);
        if (user != null)
        {
            // Get total likes for the poem
            var totalLikes = await _unitOfWork.GetRepository<Like>()
                .AsQueryable()
                .Where(p => p.PoemId == poemId)
                .CountAsync() - 1; // Exclude the current user who liked the poem
            
            // Announce to poem owner that their poem has been liked
            await _publishEndpoint.Publish(new UpdateAndSendUserAnnouncementEvent()
            {
                UserId = userId,
                Title = "Bài thơ của bạn đã được thích",
                Content = $"Bài thơ: \"{poem.Title}\" của bạn đã được thích bởi {user.UserName} và {totalLikes} người khác.",
                IsRead = false,
                Type = AnnouncementType.Like
            });
        }
    }
    
    public async Task DislikePoem(Guid userId, Guid poemId)
    {
        // Check if poem exists
        Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poemId);
        if(poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }
        
        var like = await _unitOfWork.GetRepository<Like>()
            .FindAsync(x => x.PoemId == poemId && x.UserId == userId);
        
        // If user hasn't liked the poem, return
        if (like == null)
        {
            return;
        }

        _unitOfWork.GetRepository<Like>().DeletePermanent(like);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetLikePoemResponse> GetLikePoem(Guid poemId)
    {
        // Get all likes for the poem
        var likes = await _unitOfWork.GetRepository<Like>()
            .AsQueryable()
            .Where(p => p.PoemId == poemId)
            .ToListAsync();
        
        
        return new GetLikePoemResponse()
        {
            // Map the user information
            User = likes.Select(p => p.User).Select(p => new GetBasicUserInformationResponse()
            {
                Id = p.Id,
                DisplayName = p.DisplayName,
                UserName = p.UserName,
                Avatar = p.Avatar,
            }).ToList(),
            
            // Get the total likes
            TotalLikes = likes.Count
        };
    }
}