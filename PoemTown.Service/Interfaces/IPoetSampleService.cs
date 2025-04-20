using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.PoetSampleRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PoetSampleResponses;
using PoemTown.Service.QueryOptions.FilterOptions.PoetSampleFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoetSampleSorts;

namespace PoemTown.Service.Interfaces;

public interface IPoetSampleService
{
    Task CreateNewPoetSample(CreateNewPoetSampleRequest request);
    Task UpdatePoetSample(UpdatePoetSampleRequest request);
    Task DeletePoetSample(Guid poetSampleId);
    Task<string> UploadPoetSampleAvatar(IFormFile file);

    Task<PaginationResponse<GetPoetSampleResponse>>
        GetPoetSamples(RequestOptionsBase<GetPoetSampleFilterOption, GetPoetSampleSortOption> request);

    Task<GetPoetSampleResponse> GetPoetSample(Guid poetSampleId);
    Task RemovePoetSampleTitleSample(Guid poetSampleId, IList<Guid> titleSampleIds);
}