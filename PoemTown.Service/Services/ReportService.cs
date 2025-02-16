using Microsoft.AspNetCore.Http;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Poems;
using PoemTown.Repository.Enums.Reports;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.ReportRequests;
using PoemTown.Service.Interfaces;

namespace PoemTown.Service.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    public ReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateReportPoem(Guid userId, CreateReportPoemRequest request)
    {
        // Check if the poem exists
        var poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == request.PoemId);
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }

        if (poem.Status != PoemStatus.Posted)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Can not report poem which is not posted");
        }
        
        // Check if the user reported his own poem, if so, throw an exception
        if(poem.Collection!.UserId == userId)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "You can't report your own poem");
        }
        
        var report = new Report
        {
            PoemId = request.PoemId,
            ReportReason = request.ReportReason,
            ReportUserId = userId,
        };
        
        await _unitOfWork.GetRepository<Report>().InsertAsync(report);
        await _unitOfWork.SaveChangesAsync();
    }
}