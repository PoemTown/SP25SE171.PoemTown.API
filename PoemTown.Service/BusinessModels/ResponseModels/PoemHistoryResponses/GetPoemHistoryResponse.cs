using PoemTown.Repository.Enums.Poems;
using PoemTown.Service.BusinessModels.ResponseModels.PoemTypeResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.PoemHistoryResponses;

public class GetPoemHistoryResponse
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public string Title { get; set; }
    public GetPoemTypeResponse Type { get; set; }
    public PoemStatus Status { get; set; }
    public string Description { get; set; }
}