using AutoMapper;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.WithdrawalComplaints;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.WithdrawalComplaintRequests;
using PoemTown.Service.BusinessModels.ResponseModels.WithdrawalComplaintResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.WithdrawalComplaintFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.WithdrawalComplaintSorts;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Models.AwsS3;

namespace PoemTown.Service.Services;

public class WithdrawalComplaintService : IWithdrawalComplaintService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAwsS3Service _awsS3Service;

    public WithdrawalComplaintService(IUnitOfWork unitOfWork, IMapper mapper, IAwsS3Service awsS3Service)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _awsS3Service = awsS3Service;
    }


    public async Task CreateNewWithdrawalComplaint(Guid withdrawalFormId, CreateNewWithdrawalComplaintRequest request)
    {
        var withdrawalForm = await _unitOfWork.GetRepository<WithdrawalForm>()
            .FindAsync(p => p.Id == withdrawalFormId && p.DeletedTime == null);

        // Check if the withdrawal form exists
        if (withdrawalForm == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không tìm thấy yêu cầu rút tiền");
        }

        var existPendingWithdrawalComplaint = await _unitOfWork.GetRepository<WithdrawalComplaint>()
            .FindAsync(p =>
                p.WithdrawalFormId == withdrawalFormId && p.DeletedTime == null &&
                p.Status == WithdrawalComplaintStatus.Pending);

        // Check if there is already a pending complaint for this withdrawal form
        if (existPendingWithdrawalComplaint != null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest,
                "Đã có khiếu nại đang chờ xử lý cho yêu cầu rút tiền này");
        }

        var withdrawalComplaint = _mapper.Map<WithdrawalComplaint>(request);
        withdrawalComplaint.Id = Guid.NewGuid();
        withdrawalComplaint.WithdrawalFormId = withdrawalFormId;
        withdrawalComplaint.Status = WithdrawalComplaintStatus.Pending;

        // Save withdrawal complaint images if exists images from request
        if (request.Images != null && request.Images.Count > 0)
        {
            var withdrawalComplaintImages = new List<WithdrawalComplaintImage>();
            foreach (var image in request.Images)
            {
                withdrawalComplaintImages.Add(new WithdrawalComplaintImage()
                {
                    Image = image,
                    Type = WithdrawalComplaintType.Complaint,
                    WithdrawalComplaintId = withdrawalComplaint.Id
                });
            }

            await _unitOfWork.GetRepository<WithdrawalComplaintImage>().InsertRangeAsync(withdrawalComplaintImages);
        }
        
        await _unitOfWork.GetRepository<WithdrawalComplaint>().InsertAsync(withdrawalComplaint);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task UpdateWithdrawalComplaint(UpdateWithdrawalComplaintRequest request)
    {
        var withdrawalComplaint = await _unitOfWork.GetRepository<WithdrawalComplaint>()
            .FindAsync(p => p.Id == request.Id && p.DeletedTime == null);

        // Check if the withdrawal complaint exists
        if (withdrawalComplaint == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không tìm thấy khiếu nại");
        }

        // Check if the withdrawal complaint is already resolved
        if(withdrawalComplaint.Status != WithdrawalComplaintStatus.Pending)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không thể cập nhật khiếu nại đã được xử lý");
        }
        
        // Update the withdrawal complaint
        _mapper.Map(request, withdrawalComplaint);
        _unitOfWork.GetRepository<WithdrawalComplaint>().Update(withdrawalComplaint);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<string> UploadWithdrawalComplaintImage(IFormFile file)
    {
        // Validate the file
        ImageHelper.ValidateImage(file);

        // Upload image to AWS S3
        var fileName = "withdrawal-complaint-images";
        UploadImageToAwsS3Model s3Model = new UploadImageToAwsS3Model()
        {
            File = file,
            FolderName = fileName
        };
        return await _awsS3Service.UploadImageToAwsS3Async(s3Model);
    }
    
    public async Task DeleteWithdrawalComplaint(Guid withdrawalComplaintId)
    {
        var withdrawalComplaint = await _unitOfWork.GetRepository<WithdrawalComplaint>()
            .FindAsync(p => p.Id == withdrawalComplaintId && p.DeletedTime == null);

        // Check if the withdrawal complaint exists
        if (withdrawalComplaint == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không tìm thấy khiếu nại");
        }

        if(withdrawalComplaint.Status != WithdrawalComplaintStatus.Pending)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không thể xóa khiếu nại đã được xử lý");
        }
        
        // Soft delete the withdrawal complaint
        _unitOfWork.GetRepository<WithdrawalComplaint>().Delete(withdrawalComplaint);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task AddImageToUpdateWithdrawalComplaint(Guid withdrawalComplaintId, string image)
    {
        var withdrawalComplaint = await _unitOfWork.GetRepository<WithdrawalComplaint>()
            .FindAsync(p => p.Id == withdrawalComplaintId && p.DeletedTime == null);

        // Check if the withdrawal complaint exists
        if (withdrawalComplaint == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không tìm thấy khiếu nại");
        }

        // Add image to the withdrawal complaint
        var withdrawalComplaintImage = new WithdrawalComplaintImage()
        {
            Id = Guid.NewGuid(),
            Image = image,
            Type = WithdrawalComplaintType.Complaint,
            WithdrawalComplaintId = withdrawalComplaintId
        };

        await _unitOfWork.GetRepository<WithdrawalComplaintImage>().InsertAsync(withdrawalComplaintImage);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteImageFromWithdrawalComplaint(Guid withdrawalComplaintId, Guid withdrawalComplaintImageId)
    {
        var withdrawalComplaint = await _unitOfWork.GetRepository<WithdrawalComplaint>()
            .FindAsync(p => p.Id == withdrawalComplaintId && p.DeletedTime == null);

        // Check if the withdrawal complaint exists
        if (withdrawalComplaint == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không tìm thấy khiếu nại");
        }

        var withdrawalComplaintImage = await _unitOfWork.GetRepository<WithdrawalComplaintImage>()
            .FindAsync(p => p.Id == withdrawalComplaintImageId && p.DeletedTime == null);

        // Check if the withdrawal complaint image exists
        if (withdrawalComplaintImage == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không tìm thấy hình ảnh khiếu nại");
        }

        // Soft delete the withdrawal complaint image
        _unitOfWork.GetRepository<WithdrawalComplaintImage>().DeletePermanent(withdrawalComplaintImage);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<PaginationResponse<GetWithdrawalComplaintResponse>> GetWithdrawalComplaints(
        RequestOptionsBase<GetWithdrawalComplaintFilterOption, GetWithdrawalComplaintSortOption> request)
    {
        var withdrawalComplaintQuery = _unitOfWork.GetRepository<WithdrawalComplaint>().AsQueryable();

        // IsDelete
        if (request.IsDelete == false)
        {
            withdrawalComplaintQuery = withdrawalComplaintQuery.Where(x => x.DeletedTime == null);
        }
        else
        {
            withdrawalComplaintQuery = withdrawalComplaintQuery.Where(x => x.DeletedTime != null);
        }

        // Filter
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.WithdrawalFormId != null)
            {
                withdrawalComplaintQuery = withdrawalComplaintQuery
                    .Where(x => x.WithdrawalFormId == request.FilterOptions.WithdrawalFormId);
            }

            if (request.FilterOptions.Status != null)
            {
                withdrawalComplaintQuery = withdrawalComplaintQuery
                    .Where(x => x.Status == request.FilterOptions.Status);
            }
        }
        
        // Sort
        withdrawalComplaintQuery = request.SortOptions switch
        {
            GetWithdrawalComplaintSortOption.CreatedTimeAscending => withdrawalComplaintQuery
                .OrderBy(x => x.CreatedTime),
            GetWithdrawalComplaintSortOption.CreatedTimeDescending => withdrawalComplaintQuery
                .OrderByDescending(x => x.CreatedTime),
            _ => withdrawalComplaintQuery.OrderBy(x => x.Status == WithdrawalComplaintStatus.Pending).ThenBy(x => x.CreatedTime)
        };
        
        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<WithdrawalComplaint>().GetPagination(
            withdrawalComplaintQuery,
            request.PageNumber,
            request.PageSize);

        var withdrawalComplaints = _mapper.Map<IList<GetWithdrawalComplaintResponse>>(queryPaging.Data);
        
        return new PaginationResponse<GetWithdrawalComplaintResponse>(
            withdrawalComplaints,
            queryPaging.PageNumber,
            queryPaging.PageSize,
            queryPaging.TotalPages,
            queryPaging.CurrentPageRecords);
    }

    public async Task<PaginationResponse<GetWithdrawalComplaintResponse>> GetMyWithdrawalComplaints(Guid userId,
        RequestOptionsBase<GetWithdrawalComplaintFilterOption, GetWithdrawalComplaintSortOption> request)
    {
        var withdrawalComplaintQuery = _unitOfWork.GetRepository<WithdrawalComplaint>().AsQueryable();

        withdrawalComplaintQuery = withdrawalComplaintQuery
            .Where(x => x.WithdrawalForm!.UserEWallet.User.Id == userId);
        
        // IsDelete
        if (request.IsDelete == false)
        {
            withdrawalComplaintQuery = withdrawalComplaintQuery.Where(x => x.DeletedTime == null);
        }
        else
        {
            withdrawalComplaintQuery = withdrawalComplaintQuery.Where(x => x.DeletedTime != null);
        }

        // Filter
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.WithdrawalFormId != null)
            {
                withdrawalComplaintQuery = withdrawalComplaintQuery
                    .Where(x => x.WithdrawalFormId == request.FilterOptions.WithdrawalFormId);
            }

            if (request.FilterOptions.Status != null)
            {
                withdrawalComplaintQuery = withdrawalComplaintQuery
                    .Where(x => x.Status == request.FilterOptions.Status);
            }
        }
        
        // Sort
        withdrawalComplaintQuery = request.SortOptions switch
        {
            GetWithdrawalComplaintSortOption.CreatedTimeAscending => withdrawalComplaintQuery
                .OrderBy(x => x.CreatedTime),
            GetWithdrawalComplaintSortOption.CreatedTimeDescending => withdrawalComplaintQuery
                .OrderByDescending(x => x.CreatedTime),
            _ => withdrawalComplaintQuery.OrderBy(x => x.Status == WithdrawalComplaintStatus.Pending).ThenBy(x => x.CreatedTime)
        };
        
        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<WithdrawalComplaint>().GetPagination(
            withdrawalComplaintQuery,
            request.PageNumber,
            request.PageSize);

        var withdrawalComplaints = _mapper.Map<IList<GetWithdrawalComplaintResponse>>(queryPaging.Data);
        
        return new PaginationResponse<GetWithdrawalComplaintResponse>(
            withdrawalComplaints,
            queryPaging.PageNumber,
            queryPaging.PageSize,
            queryPaging.TotalPages,
            queryPaging.CurrentPageRecords);
    }
    
    public async Task<GetWithdrawalComplaintResponse> GetWithdrawalComplaintById(Guid withdrawalComplaintId)
    {
        var withdrawalComplaint = await _unitOfWork.GetRepository<WithdrawalComplaint>()
            .FindAsync(p => p.Id == withdrawalComplaintId && p.DeletedTime == null);

        // Check if the withdrawal complaint exists
        if (withdrawalComplaint == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không tìm thấy khiếu nại");
        }

        var withdrawalComplaintResponse = _mapper.Map<GetWithdrawalComplaintResponse>(withdrawalComplaint);
        
        return withdrawalComplaintResponse;
    }
    
    public async Task ResolveWithdrawalComplaint(Guid withdrawalComplaintId, ResolveWithdrawalComplaintRequest request)
    {
        var withdrawalComplaint = await _unitOfWork.GetRepository<WithdrawalComplaint>()
            .FindAsync(p => p.Id == withdrawalComplaintId && p.DeletedTime == null);

        // Check if the withdrawal complaint exists
        if (withdrawalComplaint == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không tìm thấy khiếu nại");
        }

        // Check if the withdrawal complaint is already resolved
        if(withdrawalComplaint.Status != WithdrawalComplaintStatus.Pending)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Không thể xử lý khiếu nại đã được xử lý");
        }
        
        // Update the withdrawal complaint status and resolve description
        withdrawalComplaint.Status = request.Status;
        withdrawalComplaint.ResolveDescription = request.ResolveDescription;

        // Save withdrawal complaint images if exists images from request
        if (request.Images != null && request.Images.Count > 0)
        {
            var withdrawalComplaintImages = new List<WithdrawalComplaintImage>();
            foreach (var image in request.Images)
            {
                withdrawalComplaintImages.Add(new WithdrawalComplaintImage()
                {
                    Image = image,
                    Type = WithdrawalComplaintType.ResolveComplaint,
                    WithdrawalComplaintId = withdrawalComplaint.Id
                });
            }

            await _unitOfWork.GetRepository<WithdrawalComplaintImage>().InsertRangeAsync(withdrawalComplaintImages);
        }
        _unitOfWork.GetRepository<WithdrawalComplaint>().Update(withdrawalComplaint);
        await _unitOfWork.SaveChangesAsync();
    }
}