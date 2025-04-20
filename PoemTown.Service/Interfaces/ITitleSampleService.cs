using PoemTown.Service.BusinessModels.RequestModels.TitleSampleRequests;
using PoemTown.Service.BusinessModels.ResponseModels.TitleSampleResponses;

namespace PoemTown.Service.Interfaces;

public interface ITitleSampleService
{
    Task CreateTitleSample(CreateTitleSampleRequest request);
    Task UpdateTitleSample(UpdateTitleSampleRequest request);
    Task DeleteTitleSample(Guid id);
    Task DeleteTitleSamplePermanently(Guid id);
    Task<GetTitleSampleResponse> GetTitleSampleById(Guid id);
    Task<IEnumerable<GetTitleSampleResponse>> GetAllTitleSamples();
    Task AddTitleSamplesIntoPoetSample(Guid poetSampleId, List<Guid> titleSampleIds);
    Task RemovePoetSampleTitleSample(Guid poetSampleId, IList<Guid> titleSampleIds);
}