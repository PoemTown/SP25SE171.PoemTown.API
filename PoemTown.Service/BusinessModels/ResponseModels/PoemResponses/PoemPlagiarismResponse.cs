using PoemTown.Repository.Enums.Poems;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;

public class PoemPlagiarismResponse
{
    public double Score { get; set; }
    public bool IsPlagiarism { get; set; }
    
    public IList<PoemPlagiarismFromResponse>? PlagiarismFrom { get; set; }
}

public class PoemPlagiarismFromResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public PoemType Type { get; set; }
    public string Description { get; set; }
    public string? PoemImage { get; set; }
    public double Score { get; set; }
    public GetBasicUserInformationResponse User { get; set; }
}