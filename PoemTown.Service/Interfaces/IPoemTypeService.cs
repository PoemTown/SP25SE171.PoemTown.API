using PoemTown.Service.BusinessModels.RequestModels.PoemTypeRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PoemTypeResponses;

namespace PoemTown.Service.Interfaces;

public interface IPoemTypeService
{
    Task<IEnumerable<GetPoemTypeResponse>> GetAllPoemTypes();
    Task UpdatePoemType(UpdatePoemTypeRequest request);
    Task<GetPoemTypeResponse> GetPoemTypeById(Guid poemTypeId);
    Task CreatePoemType(CreatePoemTypeRequest request);
    Task DeletePoemType(Guid poemTypeId);
}