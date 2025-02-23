namespace PoemTown.Service.Interfaces;

public interface ILikeService
{
    Task LikePoem(Guid userId, Guid poemId);
    Task DislikePoem(Guid userId, Guid poemId);
}