using PoemTown.Service.BusinessModels.ResponseModels.PoetSampleResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TargetMarkResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;

public class GetPoetSampleCollectionResponse
{
    public Guid Id { get; set; }
    public string CollectionName { get; set; } = default!;
    public string? CollectionDescription { get; set; } = default!;
    public string? CollectionImage { get; set; } = default!;
    public bool? IsDefault { get; set; } = false;
    public int? TotalChapter { get; set; } = default!;
    public int? TotalRecord { get; set; } = default!;
    public byte[]? RowVersion { get; set; }
    public bool? IsCommunity { get; set; }
    public bool? IsFamousPoet { get; set; } = false;
    public bool? IsMine { get; set; } = false;
    public DateTimeOffset? CreatedTime { get; set; }
    public DateTimeOffset? LastUpdatedTime { get; set; }
    public GetPoetSampleResponse? PoetSample { get; set; }
    public GetTargetMarkResponse? TargetMark { get; set; }
}