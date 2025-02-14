using PoemTown.Service.BusinessModels.RequestModels.ReportRequests;

namespace PoemTown.Service.Interfaces;

public interface IReportService
{
    Task CreateReportPoem(Guid userId, CreateReportPoemRequest request);
}