using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.BankTypes;
using PoemTown.Repository.Enums.WithdrawalForm;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.BankTypeRequests;
using PoemTown.Service.BusinessModels.ResponseModels.BankTypeResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.BankTypeFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.BankTypeSorts;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Models.AwsS3;

namespace PoemTown.Service.Services;

public class BankTypeService : IBankTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAwsS3Service _awsS3Service;
    public BankTypeService(IUnitOfWork unitOfWork, IMapper mapper , IAwsS3Service awsS3Service)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _awsS3Service = awsS3Service;
    }


    public async Task CreateNewBankType(CreateNewBankTypeRequest request)
    {
        request.BankName = StringHelper.FormatStringInput(request.BankName);
        request.BankCode = StringHelper.CapitalizeString(request.BankCode);
        
        var bankType = _mapper.Map<BankType>(request);

        await _unitOfWork.GetRepository<BankType>().InsertAsync(bankType);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateBankType(UpdateBankTypeRequest request)
    {
        var bankType = await _unitOfWork.GetRepository<BankType>().FindAsync(p => p.Id == request.Id);
        
        // Check if bank type exists
        if (bankType == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Ngân hàng không tồn tại");
        }
        
        request.BankName = StringHelper.FormatStringInput(request.BankName);
        request.BankCode = StringHelper.CapitalizeString(request.BankCode);
        
        _mapper.Map(request, bankType);
        
        _unitOfWork.GetRepository<BankType>().Update(bankType);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteBankType(Guid bankTypeId)
    {
        var bankType = await _unitOfWork.GetRepository<BankType>().FindAsync(p => p.Id == bankTypeId);
        
        // Check if bank type exists
        if (bankType == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Ngân hàng không tồn tại");
        }
        
        // Check if bank type has user bank types
        var userBankTypes = await _unitOfWork.GetRepository<UserBankType>()
            .AsQueryable()
            .Where(p => p.BankTypeId == bankTypeId)
            .ToListAsync();

        if (userBankTypes != null && userBankTypes.Count > 0)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không thể xóa ngân hàng này vì đã có tài khoản ngân hàng sử dụng ngân hàng này");
        }
        
        // Check if bank type has withdrawal forms
        var withdrawalForms = await _unitOfWork.GetRepository<WithdrawalForm>()
            .AsQueryable()
            .Where(p => p.BankTypeId == bankTypeId)
            .ToListAsync();
        if (withdrawalForms != null && withdrawalForms.Count > 0)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không thể xóa ngân hàng này vì đã có giao dịch rút tiền sử dụng ngân hàng này");
        }
        
        _unitOfWork.GetRepository<BankType>().Delete(bankType);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<GetBankTypeResponse> GetBankTypeDetail(Guid bankTypeId)
    {
        var bankType = await _unitOfWork.GetRepository<BankType>().FindAsync(p => p.Id == bankTypeId);
        
        // Check if bank type exists
        if (bankType == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Ngân hàng không tồn tại");
        }
        
        var response = _mapper.Map<GetBankTypeResponse>(bankType);
        return response;
    }

    public async Task<string> UploadBankTypeImageIcon(IFormFile file)
    {
        // Validate the file
        ImageHelper.ValidateImage(file);

        // Upload image to AWS S3
        var fileName = "banktypes-icon";
        UploadImageToAwsS3Model s3Model = new UploadImageToAwsS3Model()
        {
            File = file,
            FolderName = fileName
        };
        return await _awsS3Service.UploadImageToAwsS3Async(s3Model);
    }

    public async Task<IEnumerable<GetBankTypeResponse>> UserGetBankTypes(GetBankTypeFilterOption filter)
    {
        var bankTypeQuery = _unitOfWork.GetRepository<BankType>()
            .AsQueryable()
            .Where(p => p.DeletedTime == null && p.Status == BankTypeStatus.Active);
        
        if(filter.BankName != null)
        {
            bankTypeQuery = bankTypeQuery.Where(p => p.BankName!.Trim().ToLower().Contains(filter.BankName.Trim().ToLower()));
        }
        
        if(filter.BankCode != null)
        {
            bankTypeQuery = bankTypeQuery.Where(p => p.BankCode!.Trim().ToLower().Contains(filter.BankCode.Trim().ToLower()));
        }
        
        if(filter.Status != null)
        {
            bankTypeQuery = bankTypeQuery.Where(p => p.Status == filter.Status);
        }

        var bankTypes = await bankTypeQuery.ToListAsync();
        
        var response = _mapper.Map<IEnumerable<GetBankTypeResponse>>(bankTypes);
        return response;
    }

    public async Task<PaginationResponse<GetBankTypeResponse>> 
        GetBankTypes(RequestOptionsBase<GetBankTypeFilterOption, GetBankTypeSortOption> request)
    {
        var bankTypeQuery = _unitOfWork.GetRepository<BankType>().AsQueryable();
        
        // IsDelete
        if (request.IsDelete == true)
        {
            bankTypeQuery = bankTypeQuery.Where(p => p.DeletedTime != null);
        }
        else
        {
            bankTypeQuery = bankTypeQuery.Where(p => p.DeletedTime == null);
        }
        
        // Filter
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.BankName != null)
            {
                bankTypeQuery = bankTypeQuery.Where(p => p.BankName!.Trim().ToLower().Contains(request.FilterOptions.BankName.Trim().ToLower()));
            }
            
            if (request.FilterOptions.BankCode != null)
            {
                bankTypeQuery = bankTypeQuery.Where(p => p.BankCode!.Trim().ToLower().Contains(request.FilterOptions.BankCode.Trim().ToLower()));
            }
            
            if (request.FilterOptions.Status != null)
            {
                bankTypeQuery = bankTypeQuery.Where(p => p.Status == request.FilterOptions.Status);
            }
        }
        
        // Sort
        bankTypeQuery = request.SortOptions switch
        {
            GetBankTypeSortOption.CreatedTimeAscending => bankTypeQuery.OrderBy(p => p.CreatedTime),
            GetBankTypeSortOption.CreatedTimeDescending => bankTypeQuery.OrderByDescending(p => p.CreatedTime),
            _ => bankTypeQuery.OrderBy(p => p.Status == BankTypeStatus.Active).ThenByDescending(p => p.CreatedTime)
        };
        
        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<BankType>().GetPagination(bankTypeQuery, request.PageNumber, request.PageSize);

        var bankTypes = _mapper.Map<IList<GetBankTypeResponse>>(queryPaging.Data);

        return new PaginationResponse<GetBankTypeResponse>(bankTypes, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalPages, queryPaging.CurrentPageRecords);
    }

    public async Task CreateUserBankType(Guid userId, CreateUserBankTypeRequest request)
    {
        var bankType = await _unitOfWork.GetRepository<BankType>().FindAsync(p => p.Id == request.BankTypeId && 
            p.DeletedTime == null && p.Status == BankTypeStatus.Active);
        
        // Check if bank type exists
        if (bankType == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Ngân hàng không tồn tại");
        }
        
        // Format input
        request.AccountName = StringHelper.CapitalizeString(request.AccountName);
        request.AccountNumber = request.AccountNumber.Trim();
        
        var userBankType = _mapper.Map<UserBankType>(request);
        userBankType.BankTypeId = bankType.Id;
        userBankType.UserId = userId;
        
        await _unitOfWork.GetRepository<UserBankType>().InsertAsync(userBankType);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task UpdateUserBankType(Guid userId, UpdateUserBankTypeRequest request)
    {
        var existUserBankType = await _unitOfWork.GetRepository<UserBankType>().FindAsync(p => p.Id == request.Id);
        
        // Check if user bank type exists
        if(existUserBankType == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Tài khoản ngân hàng không tồn tại");
        }
        
        // Check if user owns the bank type
        if (existUserBankType.UserId != userId)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Người dùng không sở hữu tài khoản ngân hàng này");
        }
        
        var bankType = await _unitOfWork.GetRepository<BankType>().FindAsync(p => p.Id == request.BankTypeId && 
            p.DeletedTime == null && p.Status == BankTypeStatus.Active);
        
        // Check if bank type exists
        if (bankType == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Ngân hàng không tồn tại");
        }
        
        // Check if user bank type is already used in withdrawal forms which are accepted
        var isExistWithdrawalForms = await _unitOfWork.GetRepository<WithdrawalForm>()
            .AsQueryable()
            .AnyAsync(p => p.UserBankTypeId == existUserBankType.Id && p.Status == WithdrawalFormStatus.Accepted);
        if (isExistWithdrawalForms)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không thể cập nhật tài khoản ngân hàng này vì đã có giao dịch rút tiền sử dụng tài khoản này");
        }
        
        // Format input
        request.AccountName = StringHelper.CapitalizeString(request.AccountName);
        request.AccountNumber = request.AccountNumber.Trim();
        
        _mapper.Map(request, existUserBankType);
        existUserBankType.UserId = userId;
            
        _unitOfWork.GetRepository<UserBankType>().Update(existUserBankType);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteUserBankType(Guid userBankTypeId)
    {
        var userBankType = await _unitOfWork.GetRepository<UserBankType>().FindAsync(p => p.Id == userBankTypeId);
        
        // Check if user bank type exists
        if(userBankType == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Tài khoản ngân hàng không tồn tại");
        }
        
        // Check if user bank type has withdrawal forms
        var withdrawalForms = await _unitOfWork.GetRepository<WithdrawalForm>()
            .AsQueryable()
            .Where(p => p.UserBankTypeId == userBankTypeId)
            .ToListAsync();
        
        if (withdrawalForms != null && withdrawalForms.Count > 0)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không thể xóa tài khoản ngân hàng này vì đã có giao dịch rút tiền sử dụng tài khoản này"); 
        }
        
        _unitOfWork.GetRepository<UserBankType>().Delete(userBankType);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<GetUserBankTypeResponse> GetUserBankTypeDetail(Guid userBankTypeId)
    {
        var userBankType = await _unitOfWork.GetRepository<UserBankType>()
            .FindAsync(p => p.Id == userBankTypeId);
        
        // Check if user bank type exists
        if(userBankType == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Tài khoản ngân hàng không tồn tại");
        }
        
        var response = _mapper.Map<GetUserBankTypeResponse>(userBankType);
        return response;
    }
    
    public async Task<PaginationResponse<GetUserBankTypeResponse>> GetUserBankTypes
        (Guid userId, RequestOptionsBase<GetUserBankTypeFilterOption, GetUserBankTypeSortOption> request)
    {
        var userBankTypeQuery = _unitOfWork.GetRepository<UserBankType>().AsQueryable();

        userBankTypeQuery = userBankTypeQuery.Where(p => p.UserId == userId);
        // IsDelete
        if (request.IsDelete == true)
        {
            userBankTypeQuery = userBankTypeQuery.Where(p => p.DeletedTime != null);
        }
        else
        {
            userBankTypeQuery = userBankTypeQuery.Where(p => p.DeletedTime == null);
        }
        
        // Filter
        if (request.FilterOptions != null)
        {
            if (!String.IsNullOrWhiteSpace(request.FilterOptions.BankName))
            {
                userBankTypeQuery = userBankTypeQuery
                    .Where(p => p.BankType!.BankName!.Trim().ToLower().Contains(request.FilterOptions.BankName.Trim().ToLower()));
            }
            if (!String.IsNullOrWhiteSpace(request.FilterOptions.BankCode))
            {
                userBankTypeQuery = userBankTypeQuery
                    .Where(p => p.AccountName!.Trim().ToLower().Contains(request.FilterOptions.BankCode.Trim().ToLower()));
            }
        }
        
        // Sort
        userBankTypeQuery = request.SortOptions switch
        {
            GetUserBankTypeSortOption.CreatedTimeAscending => userBankTypeQuery.OrderBy(p => p.CreatedTime),
            GetUserBankTypeSortOption.CreatedTimeDescending => userBankTypeQuery.OrderByDescending(p => p.CreatedTime),
            _ => userBankTypeQuery.OrderByDescending(p => p.CreatedTime)
        };
        
        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<UserBankType>().GetPagination(userBankTypeQuery, request.PageNumber, request.PageSize);

        var bankTypes = _mapper.Map<IList<GetUserBankTypeResponse>>(queryPaging.Data);

        return new PaginationResponse<GetUserBankTypeResponse>(bankTypes, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalPages, queryPaging.CurrentPageRecords);
    }
}