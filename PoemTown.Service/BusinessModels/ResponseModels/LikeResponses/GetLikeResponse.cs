namespace PoemTown.Service.BusinessModels.ResponseModels.LikeResponses;

public class GetLikeResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PoemId { get; set; }
}