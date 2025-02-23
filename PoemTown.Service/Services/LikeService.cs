using Microsoft.AspNetCore.Http;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Services;

public class LikeService : ILikeService
{
    private readonly IUnitOfWork _unitOfWork;

    public LikeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
}