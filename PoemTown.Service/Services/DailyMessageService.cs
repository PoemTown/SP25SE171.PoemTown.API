using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.DailyMessageRequests;
using PoemTown.Service.BusinessModels.ResponseModels.DailyMessageResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.DailyMessageFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.DailyMessageSorts;

namespace PoemTown.Service.Services;

public class DailyMessageService : IDailyMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public DailyMessageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task CreateNewDailyMessage(CreateNewDailyMessageRequest request)
    {
        DailyMessage dailyMessage = _mapper.Map<DailyMessage>(request.Message);
        dailyMessage.Id = Guid.NewGuid();
        
        await UpdateInUseDailyMessage(dailyMessage);
        
        await _unitOfWork.GetRepository<DailyMessage>().InsertAsync(dailyMessage);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task UpdateInUseDailyMessage(DailyMessage dailyMessage)
    {
        // If there is no daily message in use, set the new one to be in use
        if(dailyMessage.IsInUse == true)
        {
            // If the daily message is set to be in use, set all other messages to not in use
            var allDailyMessages = await _unitOfWork.GetRepository<DailyMessage>()
                .AsQueryable()
                .Where(p => p.Id != dailyMessage.Id)
                .ToListAsync();

            foreach (var message in allDailyMessages)
            {
                message.IsInUse = false;
            }
            
            _unitOfWork.GetRepository<DailyMessage>().UpdateRange(allDailyMessages);
        }
        else
        {
            // If the daily message is not in use, check if there are any other messages in use
            var existInUseDailyMessage = await _unitOfWork.GetRepository<DailyMessage>()
                .AsQueryable()
                .AnyAsync(p => p.IsInUse == true);
            
            // If there are no other messages in use, set the current one to be in use
            if (!existInUseDailyMessage)
            {
                dailyMessage.IsInUse = true;
            }
        }
    }
    
    public async Task UpdateDailyMessage(UpdateDailyMessageRequest request)
    {
        DailyMessage? dailyMessage = await _unitOfWork.GetRepository<DailyMessage>()
            .FindAsync(p => p.Id == request.Id);

        // Check if the daily message exists
        if (dailyMessage == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Daily message not found");
        }

        _mapper.Map(request, dailyMessage);
        
        await UpdateInUseDailyMessage(dailyMessage);
        
        _unitOfWork.GetRepository<DailyMessage>().Update(dailyMessage);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteDailyMessage(Guid dailyMessageId)
    {
        DailyMessage? dailyMessage = await _unitOfWork.GetRepository<DailyMessage>()
            .FindAsync(p => p.Id == dailyMessageId);

        // Check if the daily message exists
        if (dailyMessage == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Daily message not found");
        }

        // If the daily message is in use, set the IsInUse property to false
        if (dailyMessage.IsInUse == true)
        {
            var allDailyMessages = await _unitOfWork.GetRepository<DailyMessage>()
                .AsQueryable()
                .Where(p => p.Id != dailyMessage.Id)
                .ToListAsync();
            
            // Set random message to be in use if delete the current message which IsInUse = true
            var random = new Random();
            var randomMessage = allDailyMessages[random.Next(allDailyMessages.Count)];
            randomMessage.IsInUse = true;

            _unitOfWork.GetRepository<DailyMessage>().Update(randomMessage);
        }

        dailyMessage.IsInUse = false;
        _unitOfWork.GetRepository<DailyMessage>().Delete(dailyMessage);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<GetDailyMessageResponse> GetDailyMessage(Guid dailyMessageId)
    {
        DailyMessage? dailyMessage = await _unitOfWork.GetRepository<DailyMessage>()
            .FindAsync(p => p.Id == dailyMessageId);

        // Check if the daily message exists
        if (dailyMessage == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Daily message not found");
        }

        return _mapper.Map<GetDailyMessageResponse>(dailyMessage);
    }

    public async Task<GetDailyMessageResponse?> GetInUseDailyMessage()
    {
        DailyMessage? dailyMessage = await _unitOfWork.GetRepository<DailyMessage>()
            .FindAsync(p => p.IsInUse == true && p.DeletedTime == null);

        // Check if the daily message exists
        if (dailyMessage == null)
        {
            return null;
        }

        return _mapper.Map<GetDailyMessageResponse>(dailyMessage);
    }
    
    public async Task<PaginationResponse<GetDailyMessageResponse>> 
        GetDailyMessages(RequestOptionsBase<GetDailyMessageFilterOption, GetDailyMessageSortOption> request)
    {
        var dailyMessageQuery = _unitOfWork.GetRepository<DailyMessage>().AsQueryable();
        
        // IsDelete
        if (request.IsDelete == true)
        {
            dailyMessageQuery = dailyMessageQuery.Where(p => p.DeletedTime != null);
        }
        else
        {
            dailyMessageQuery = dailyMessageQuery.Where(p => p.DeletedTime == null);
        }
        
        // Filter
        if (request.FilterOptions != null)
        {
        }
        
        // Sort
        dailyMessageQuery = request.SortOptions switch
        {
            GetDailyMessageSortOption.CreatedTimeAscending => dailyMessageQuery.OrderBy(p => p.CreatedTime),
            GetDailyMessageSortOption.CreatedTimeDescending => dailyMessageQuery.OrderByDescending(p => p.CreatedTime),
            _ => dailyMessageQuery.OrderByDescending(p => p.IsInUse).ThenByDescending(p => p.CreatedTime)
        };
        
        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<DailyMessage>()
            .GetPagination(dailyMessageQuery, request.PageNumber, request.PageSize);

        var dailyMessages = _mapper.Map<IList<GetDailyMessageResponse>>(queryPaging.Data);

        return new PaginationResponse<GetDailyMessageResponse>(dailyMessages, queryPaging.PageNumber,
            queryPaging.PageSize, queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }
}