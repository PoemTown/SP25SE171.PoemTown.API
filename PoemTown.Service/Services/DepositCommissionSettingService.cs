using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.DepositCommissionSettingRequests;
using PoemTown.Service.BusinessModels.ResponseModels.DepositCommissionSettingResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.DepositCommissionSettingFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.DepositCommissionSettingSorts;

namespace PoemTown.Service.Services;

public class DepositCommissionSettingService : IDepositCommissionSettingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DepositCommissionSettingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateNewDepositCommissionSetting(CreateNewDepositCommissionSettingRequest request)
    {
        DepositCommissionSetting newDepositCommissionSetting = new()
        {
            AmountPercentage = request.AmountPercentage,
            IsInUse = request.IsInUse
        };

        // Update existing settings if IsInUse is true into false
        if (request.IsInUse == true)
        {
            var currentDepositCommissionSettings = await _unitOfWork.GetRepository<DepositCommissionSetting>()
                .AsQueryable()
                .Where(x => x.IsInUse == true && x.DeletedTime == null)
                .ToListAsync();

            foreach (var currentDepositCommissionSetting in currentDepositCommissionSettings)
            {
                currentDepositCommissionSetting.IsInUse = false;
            }

            _unitOfWork.GetRepository<DepositCommissionSetting>().UpdateRange(currentDepositCommissionSettings);
        }

        await _unitOfWork.GetRepository<DepositCommissionSetting>().InsertAsync(newDepositCommissionSetting);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateDepositCommissionSetting(UpdateDepositCommissionSettingRequest request)
    {
        var depositCommissionSetting = await _unitOfWork.GetRepository<DepositCommissionSetting>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeletedTime == null);

        if (depositCommissionSetting == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest,
                "Không tìm thấy cấu hình phần trăm hoa hồng nạp tiền.");
        }

        depositCommissionSetting.AmountPercentage = request.AmountPercentage;
        depositCommissionSetting.IsInUse = request.IsInUse;

        // Update existing settings if IsInUse is true into false
        if (request.IsInUse == true)
        {
            var currentDepositCommissionSettings = await _unitOfWork.GetRepository<DepositCommissionSetting>()
                .AsQueryable()
                .Where(x => x.IsInUse == true && x.DeletedTime == null)
                .ToListAsync();

            foreach (var currentDepositCommissionSetting in currentDepositCommissionSettings)
            {
                currentDepositCommissionSetting.IsInUse = false;
            }

            _unitOfWork.GetRepository<DepositCommissionSetting>().UpdateRange(currentDepositCommissionSettings);
        }

        _unitOfWork.GetRepository<DepositCommissionSetting>().Update(depositCommissionSetting);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteDepositCommissionSetting(Guid id)
    {
        var depositCommissionSetting = await _unitOfWork.GetRepository<DepositCommissionSetting>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedTime == null);

        // Check if the deposit commission setting exists
        if (depositCommissionSetting == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest,
                "Không tìm thấy cấu hình phần trăm hoa hồng nạp tiền.");
        }

        _unitOfWork.GetRepository<DepositCommissionSetting>().Delete(depositCommissionSetting);
        
        // Check if there are any other settings, then select first and update into use
        var updateFirstDepositCommissionSetting = await _unitOfWork.GetRepository<DepositCommissionSetting>()
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id != id && x.DeletedTime == null);
        
        if (updateFirstDepositCommissionSetting != null)
        {
            updateFirstDepositCommissionSetting.IsInUse = true;
            _unitOfWork.GetRepository<DepositCommissionSetting>().Update(updateFirstDepositCommissionSetting);
        }
        
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PaginationResponse<GetDepositCommissionSettingsResponse>>
        GetDepositCommissionSettings(
            RequestOptionsBase<GetDepositCommissionSettingFilterOption, GetDepositCommissionSettingSortOption> request)
    {
        var depositCommissionSettingQuery = _unitOfWork.GetRepository<DepositCommissionSetting>().AsQueryable();

        // IsDelete
        if (request.IsDelete == false)
        {
            depositCommissionSettingQuery = depositCommissionSettingQuery.Where(x => x.DeletedTime == null);
        }
        else
        {
            depositCommissionSettingQuery = depositCommissionSettingQuery.Where(x => x.DeletedTime != null);
        }

        // Filter
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.AmountPercentage != null)
            {
                depositCommissionSettingQuery = depositCommissionSettingQuery
                    .Where(x => x.AmountPercentage == request.FilterOptions.AmountPercentage);
            }

            if (request.FilterOptions.IsInUse != null)
            {
                depositCommissionSettingQuery = depositCommissionSettingQuery
                    .Where(x => x.IsInUse == request.FilterOptions.IsInUse);
            }
        }
        
        // Sort
        depositCommissionSettingQuery = request.SortOptions switch
        {
            GetDepositCommissionSettingSortOption.AmountPercentageAscending => depositCommissionSettingQuery
                .OrderBy(x => x.AmountPercentage),
            GetDepositCommissionSettingSortOption.AmountPercentageDescending => depositCommissionSettingQuery
                .OrderByDescending(x => x.AmountPercentage),
            GetDepositCommissionSettingSortOption.CreatedTimeAscending => depositCommissionSettingQuery
                .OrderBy(x => x.CreatedTime),
            GetDepositCommissionSettingSortOption.CreatedTimeDescending => depositCommissionSettingQuery
                .OrderByDescending(x => x.CreatedTime),
            _ => depositCommissionSettingQuery.OrderByDescending(x => x.IsInUse == true)
                .ThenByDescending(x => x.CreatedTime)
        };
        
        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<DepositCommissionSetting>().GetPagination(
            depositCommissionSettingQuery,
            request.PageNumber,
            request.PageSize);

        var depositCommissionSettings = _mapper.Map<IList<GetDepositCommissionSettingsResponse>>(queryPaging.Data);
        
        return new PaginationResponse<GetDepositCommissionSettingsResponse>(
            depositCommissionSettings,
            queryPaging.PageNumber,
            queryPaging.PageSize,
            queryPaging.TotalPages,
            queryPaging.CurrentPageRecords);
    }
    
    public async Task<GetDepositCommissionSettingsResponse> GetDepositCommissionSettingDetail(Guid id)
    {
        var depositCommissionSetting = await _unitOfWork.GetRepository<DepositCommissionSetting>()
            .FindAsync(x => x.Id == id && x.DeletedTime == null);

        if (depositCommissionSetting == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest,
                "Không tìm thấy cấu hình phần trăm hoa hồng nạp tiền.");
        }

        return _mapper.Map<GetDepositCommissionSettingsResponse>(depositCommissionSetting);
    }
    
    public async Task<GetDepositCommissionSettingsResponse> GetInUseDepositCommissionSetting()
    {
        var depositCommissionSetting = await _unitOfWork.GetRepository<DepositCommissionSetting>()
            .FindAsync(x => x.DeletedTime == null && x.IsInUse == true);

        if (depositCommissionSetting == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest,
                "Không tìm thấy cấu hình phần trăm hoa hồng nạp tiền.");
        }

        return _mapper.Map<GetDepositCommissionSettingsResponse>(depositCommissionSetting);
    }
}