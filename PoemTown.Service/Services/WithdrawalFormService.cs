using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Announcements;
using PoemTown.Repository.Enums.WithdrawalForm;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.WithdrawalFormRequests;
using PoemTown.Service.BusinessModels.ResponseModels.WithdrawalFormResponses;
using PoemTown.Service.Events.AnnouncementEvents;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.WithdrawalFormFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.WithdrawalFormSorts;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Models.AwsS3;

namespace PoemTown.Service.Services;

public class WithdrawalFormService : IWithdrawalFormService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAwsS3Service _awsS3Service;

    public WithdrawalFormService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IPublishEndpoint publishEndpoint,
        IAwsS3Service awsS3Service)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _awsS3Service = awsS3Service;
    }

    public async Task<PaginationResponse<GetWithdrawalFormResponse>> GetWithdrawalForms(
        RequestOptionsBase<GetWithdrawalFormFilterOption, GetWithdrawalFormSortOption> request)
    {
        var withdrawalFormQuery = _unitOfWork.GetRepository<WithdrawalForm>().AsQueryable();

        // Apply filters
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.Status != null)
            {
                withdrawalFormQuery = withdrawalFormQuery.Where(p => p.Status == request.FilterOptions.Status);
            }

            if (!string.IsNullOrEmpty(request.FilterOptions.UserName))
            {
                withdrawalFormQuery = withdrawalFormQuery.Where(p => p.UserEWallet.User.UserName!.Trim().ToLower()
                    .Contains(request.FilterOptions.UserName.Trim().ToLower()));
            }
        }

        // Apply sorting
        withdrawalFormQuery = request.SortOptions switch
        {
            GetWithdrawalFormSortOption.CreatedTimeAscending => withdrawalFormQuery.OrderBy(p => p.CreatedTime),
            GetWithdrawalFormSortOption.CreatedTimeDescending => withdrawalFormQuery.OrderByDescending(p =>
                p.CreatedTime),
            _ => withdrawalFormQuery.OrderByDescending(p => p.CreatedTime)
        };

        // Apply pagination
        var queryPaging = await _unitOfWork.GetRepository<WithdrawalForm>()
            .GetPagination(withdrawalFormQuery, request.PageNumber, request.PageSize);

        var withdrawalFormResponses = _mapper.Map<List<GetWithdrawalFormResponse>>(queryPaging.Data);

        return new PaginationResponse<GetWithdrawalFormResponse>(withdrawalFormResponses, queryPaging.PageNumber,
            queryPaging.PageSize, queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }

    public async Task<PaginationResponse<GetWithdrawalFormResponse>> GetMyWithdrawalForms(Guid userId,
        RequestOptionsBase<GetWithdrawalFormFilterOption, GetWithdrawalFormSortOption> request)
    {
        User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);

        // Check if user exists
        if (user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }

        UserEWallet? userEWallet = await _unitOfWork.GetRepository<UserEWallet>().FindAsync(p => p.UserId == userId);

        // Check if user e-wallet exists
        if (userEWallet == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User e-wallet not found");
        }

        var withdrawalFormQuery = _unitOfWork.GetRepository<WithdrawalForm>().AsQueryable();

        withdrawalFormQuery = withdrawalFormQuery.Where(p => p.UserEWalletId == userEWallet.Id);

        // Apply filters
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.Status != null)
            {
                withdrawalFormQuery = withdrawalFormQuery.Where(p => p.Status == request.FilterOptions.Status);
            }

            if (!string.IsNullOrEmpty(request.FilterOptions.UserName))
            {
                withdrawalFormQuery = withdrawalFormQuery.Where(p => p.UserEWallet.User.UserName!.Trim().ToLower()
                    .Contains(request.FilterOptions.UserName.Trim().ToLower()));
            }
        }

        // Apply sorting
        withdrawalFormQuery = request.SortOptions switch
        {
            GetWithdrawalFormSortOption.CreatedTimeAscending => withdrawalFormQuery.OrderBy(p => p.CreatedTime),
            GetWithdrawalFormSortOption.CreatedTimeDescending => withdrawalFormQuery.OrderByDescending(p =>
                p.CreatedTime),
            _ => withdrawalFormQuery.OrderByDescending(p => p.CreatedTime)
        };

        // Apply pagination
        var queryPaging = await _unitOfWork.GetRepository<WithdrawalForm>()
            .GetPagination(withdrawalFormQuery, request.PageNumber, request.PageSize);

        var withdrawalFormResponses = _mapper.Map<List<GetWithdrawalFormResponse>>(queryPaging.Data);

        return new PaginationResponse<GetWithdrawalFormResponse>(withdrawalFormResponses, queryPaging.PageNumber,
            queryPaging.PageSize, queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }

    public async Task ResolveWithdrawalForm(ResolveWithdrawalFormRequest request)
    {
        WithdrawalForm? withdrawalForm = await _unitOfWork.GetRepository<WithdrawalForm>()
            .FindAsync(p => p.Id == request.Id);

        // Check if withdrawal form exists
        if (withdrawalForm == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Withdrawal form not found");
        }

        // Check if withdrawal form is not pending
        if (withdrawalForm.Status != WithdrawalFormStatus.Pending)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Withdrawal form is not pending");
        }

        withdrawalForm.ResolveDescription = request.ResolveDescription;
        withdrawalForm.ResolveEvidence = request.ResolveEvidence;
        withdrawalForm.Status = request.Status;
        _unitOfWork.GetRepository<WithdrawalForm>().Update(withdrawalForm);
        
        // Refunds to user e-wallet if withdrawal form is rejected
        if (request.Status == WithdrawalFormStatus.Rejected)
        {
            UserEWallet? userEWallet = await _unitOfWork.GetRepository<UserEWallet>()
                .FindAsync(p => p.Id == withdrawalForm.UserEWalletId);

            // Check if user e-wallet exists
            if (userEWallet == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "User e-wallet not found");
            }

            userEWallet.WalletBalance += withdrawalForm.Amount;
            _unitOfWork.GetRepository<UserEWallet>().Update(userEWallet);
        }
        await _unitOfWork.SaveChangesAsync();
        
        // Announce to user
        if (request.Status == WithdrawalFormStatus.Accepted)
        {
            await _publishEndpoint.Publish(new SendUserAnnouncementEvent()
            {
                Title = "Đơn rút tiền",
                Content = "Đơn rút tiền của bạn đã được chấp nhận, vui lòng kiểm tra lại số dư tài khoản của bạn",
                UserId = withdrawalForm.UserEWallet.UserId,
                IsRead = false,
                Type = AnnouncementType.WithdrawalForm,
                WithdrawalFormId = withdrawalForm.Id
            });
        }
        else if (request.Status == WithdrawalFormStatus.Rejected)
        {
            await _publishEndpoint.Publish(new SendUserAnnouncementEvent()
            {
                Title = "Đơn rút tiền",
                Content = "Đơn rút tiền của bạn đã bị từ chối, chúng tôi đã hoàn lại số tiền về ví của bạn",
                UserId = withdrawalForm.UserEWallet.UserId,
                IsRead = false,
                Type = AnnouncementType.WithdrawalForm,
                WithdrawalFormId = withdrawalForm.Id
            });
        }
    }
    
    public async Task<GetWithdrawalFormResponse> GetWithdrawalFormDetail(Guid id)
    {
        WithdrawalForm? withdrawalForm = await _unitOfWork.GetRepository<WithdrawalForm>()
            .FindAsync(p => p.Id == id);

        // Check if withdrawal form exists
        if (withdrawalForm == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Withdrawal form not found");
        }

        return  _mapper.Map<GetWithdrawalFormResponse>(withdrawalForm);
    }

    public async Task<string> UploadWithdrawalFormEvidence(IFormFile file)
    {
        // Validate the file
        ImageHelper.ValidateImage(file);

        // Upload image to AWS S3
        var fileName = "withdrawal-form-evidence";
        UploadImageToAwsS3Model s3Model = new UploadImageToAwsS3Model()
        {
            File = file,
            FolderName = fileName
        };
        return await _awsS3Service.UploadImageToAwsS3Async(s3Model);
    }
}