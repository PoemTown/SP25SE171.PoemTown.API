using PoemTown.Repository.Enums.Poems;

namespace PoemTown.Service.BusinessModels.ResponseModels.PoemHistoryResponses;

public class GetPoemHistoryResponse
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public string Title { get; set; }
    public PoemType Type { get; set; }
    public string Description { get; set; }
}