using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;

namespace PoemTown.Service.Interfaces;

public interface IPoemService
{
    Task CreateNewPoem(Guid userId, CreateNewPoemRequest request);
}