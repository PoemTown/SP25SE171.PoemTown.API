using PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;

namespace PoemTown.Service.Interfaces;

public interface ITemplateService
{
    Task CreateMasterTemplate(CreateMasterTemplateRequest request);
}