using AutoMapper;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.PoetSampleRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PoetSampleResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PoetSampleFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoetSampleSorts;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Models.AwsS3;

namespace PoemTown.Service.Services;

public class PoetSampleService : IPoetSampleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAwsS3Service _awsS3Service;
    
    public PoetSampleService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IAwsS3Service awsS3Service)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _awsS3Service = awsS3Service;
    }

    public async Task CreateNewPoetSample(CreateNewPoetSampleRequest request)
    {
        PoetSample poetSample = _mapper.Map<PoetSample>(request);
        
        Guid poetSampleId = Guid.NewGuid();
        
        poetSample.Id = poetSampleId;
        
        // Format the name
        poetSample.Name = StringHelper.FormatStringInput(poetSample.Name);
        
        await _unitOfWork.GetRepository<PoetSample>().InsertAsync(poetSample);
        
        // Create default collection
        Collection collection = new Collection
        {
            PoetSampleId = poetSampleId,
            CollectionName = "Bộ sưu tập mặc định",
            CollectionDescription = "Bộ sưu tập được khởi tạo mặc định bởi hệ thống",
            IsDefault = true
        };
        
        await _unitOfWork.GetRepository<Collection>().InsertAsync(collection);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdatePoetSample(UpdatePoetSampleRequest request)
    {
        PoetSample? existPoetSample = await _unitOfWork.GetRepository<PoetSample>().FindAsync(p => p.Id == request.Id);

        // Check if the poet sample exists
        if (existPoetSample == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "PoetSample is not exist");
        }
        
        existPoetSample = _mapper.Map(request, existPoetSample);
        
        // Format the name
        existPoetSample.Name = StringHelper.FormatStringInput(request.Name);
        
        _unitOfWork.GetRepository<PoetSample>().Update(existPoetSample);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeletePoetSample(Guid poetSampleId)
    {
        PoetSample? poetSample = await _unitOfWork.GetRepository<PoetSample>().FindAsync(p => p.Id == poetSampleId);
        
        // Check if the poet sample exists
        if (poetSample == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "PoetSample is not exist");
        }
        
        _unitOfWork.GetRepository<PoetSample>().Delete(poetSample);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<string> UploadPoetSampleAvatar(IFormFile file)
    {
        // Validate the file
        ImageHelper.ValidateImage(file);

        // Upload image to AWS S3
        var fileName = "sample-poets";
        UploadImageToAwsS3Model s3Model = new UploadImageToAwsS3Model()
        {
            File = file,
            FolderName = fileName
        };
        return await _awsS3Service.UploadImageToAwsS3Async(s3Model);
    }

    public async Task<PaginationResponse<GetPoetSampleResponse>>
        GetPoetSamples(RequestOptionsBase<GetPoetSampleFilterOption, GetPoetSampleSortOption> request)
    {
        var poetSampleQuery = _unitOfWork.GetRepository<PoetSample>().AsQueryable();
        
        // IsDelete
        if (request.IsDelete == true)
        {
            poetSampleQuery = poetSampleQuery.Where(p => p.DeletedTime != null);
        }
        
        else
        {
            poetSampleQuery = poetSampleQuery.Where(p => p.DeletedTime == null);
        }
        
        // Filter
        if (request.FilterOptions != null)
        {
            if(request.FilterOptions.Name != null)
            {
                poetSampleQuery = poetSampleQuery.Where(p => p.Name.ToLower().Trim().Contains(request.FilterOptions.Name.ToLower().Trim()));
            }
        }
        
        // Sort
        poetSampleQuery = request.SortOptions switch
        {
            GetPoetSampleSortOption.CreatedTimeAscending => poetSampleQuery.OrderBy(p => p.CreatedTime),
            GetPoetSampleSortOption.CreatedTimeDescending => poetSampleQuery.OrderByDescending(p => p.CreatedTime),
            _ => poetSampleQuery.OrderByDescending(p => p.CreatedTime)
        };
        
        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<PoetSample>()
            .GetPagination(poetSampleQuery, request.PageNumber, request.PageSize);
        
        var poetSamples = _mapper.Map<IList<GetPoetSampleResponse>>(queryPaging.Data);

        return new PaginationResponse<GetPoetSampleResponse>(poetSamples, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }
}