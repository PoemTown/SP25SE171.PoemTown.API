using PoemTown.Service.BusinessModels.ResponseModels.PoemTypeResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;

public class GetPoemDuplicatedResponse
{
    public double Score { get; set; }
    public bool IsDuplicated { get; set; }
    public ICollection<PoemDuplicatedFromResponse>? DuplicatedFrom { get; set; }
}

public class PoemDuplicatedFromResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public GetPoemTypeResponse Type { get; set; }
    public string Description { get; set; }
    public string? PoemImage { get; set; }
    public double Score { get; set; }
    public GetBasicUserInformationResponse? User { get; set; }
}