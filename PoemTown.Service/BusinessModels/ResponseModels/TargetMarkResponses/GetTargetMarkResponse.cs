using PoemTown.Repository.Enums.TargetMarks;

namespace PoemTown.Service.BusinessModels.ResponseModels.TargetMarkResponses;

public class GetTargetMarkResponse
{
    public Guid Id { get; set; }
    public TargetMarkType Type { get; set; }
    public Guid? CollectionId { get; set; }
    public Guid? PoemId { get; set; }
    public Guid MarkByUserId { get; set; }
}