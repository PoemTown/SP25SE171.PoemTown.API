using Microsoft.AspNetCore.Http;
using PoemTown.Service.BusinessModels.RequestModels.SystemContactRequests;
using PoemTown.Service.BusinessModels.ResponseModels.SystemContactResponses;

namespace PoemTown.Service.Interfaces;

public interface ISystemContactService
{
    Task CreateNewSystemContact(CreateNewSystemContactRequest request);
    Task UpdateSystemContact(UpdateSystemContactRequest request);
    Task DeleteSystemContact(Guid id);
    Task<GetSystemContactResponse> GetSystemContactById(Guid id);
    Task<IEnumerable<GetSystemContactResponse>> GetAllSystemContacts();
    Task<string> UploadSystemContactIcon(IFormFile file);
}