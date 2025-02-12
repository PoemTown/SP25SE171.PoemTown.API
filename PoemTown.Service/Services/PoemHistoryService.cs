using AutoMapper;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.ResponseModels.PoemHistoryResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PoemHistoryFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemHistorySorts;

namespace PoemTown.Service.Services;

public class PoemHistoryService : IPoemHistoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public PoemHistoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<PaginationResponse<GetPoemHistoryResponse>> GetPoemHistories
        (Guid poemId, RequestOptionsBase<GetPoemHistoryFilterOption, GetPoemHistorySortOptions> request)
    {
        // Check if poem exists
        Poem? poem = await _unitOfWork.GetRepository<Poem>().FindAsync(p => p.Id == poemId);
        if (poem == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Poem not found");
        }
        
        var poemHistoryQuery = _unitOfWork.GetRepository<PoemHistory>().AsQueryable();

        poemHistoryQuery = poemHistoryQuery.Where(p => p.PoemId == poemId);

        // Filter by isDelete
        if (request.IsDelete == false)
        {
            poemHistoryQuery = poemHistoryQuery.Where(p => p.DeletedTime == null);
        }
        else
        {
            poemHistoryQuery = poemHistoryQuery.Where(p => p.DeletedTime != null);
        }
        
        // Filter by FilterOptions
        if(request.FilterOptions != null)
        {
        }

        // Sort by SortOptions
        poemHistoryQuery = request.SortOptions switch
        {
            GetPoemHistorySortOptions.CreatedTimeAscending => poemHistoryQuery.OrderBy(p => p.CreatedTime),
            GetPoemHistorySortOptions.CreatedTimeDescending => poemHistoryQuery.OrderByDescending(p => p.CreatedTime),
            _ => poemHistoryQuery.OrderByDescending(p => p.CreatedTime)
        };
        
        // Paging
        var queryPaging = await _unitOfWork.GetRepository<PoemHistory>()
            .GetPagination(poemHistoryQuery, request.PageNumber, request.PageSize);
        
        var poemHistory = _mapper.Map<IList<GetPoemHistoryResponse>>(queryPaging.Data);

        return new PaginationResponse<GetPoemHistoryResponse>(poemHistory, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }
}