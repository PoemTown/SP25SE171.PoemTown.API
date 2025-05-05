using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.PoetSampleRequests;
using PoemTown.Service.BusinessModels.ResponseModels.PoetSampleResponses;
using PoemTown.Service.BusinessModels.ResponseModels.TitleSampleResponses;
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

        // Add title sample
        if (request.TitleSampleIds != null)
        {
            foreach (var titleSampleId in request.TitleSampleIds)
            {
                await AddPoetSampleTitleSample(poetSample.Id, titleSampleId);
            }
        }

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

    private async Task AddPoetSampleTitleSample(Guid poetSampleId, Guid titleSampleId)
    {
        var titleSamples = await _unitOfWork.GetRepository<TitleSample>().FindAsync(p => p.Id == titleSampleId);

        // Check if the title sample exists
        if (titleSamples == null)
        {
            return;
        }

        var poetSampleTitleSample = await _unitOfWork.GetRepository<PoetSampleTitleSample>()
            .FindAsync(p => p.TitleSampleId == titleSampleId && p.PoetSampleId == poetSampleId);

        // Check if the poet sample title sample already exists
        if (poetSampleTitleSample != null)
        {
            return;
        }

        // Insert the title sample
        poetSampleTitleSample = new PoetSampleTitleSample()
        {
            TitleSampleId = titleSampleId,
            PoetSampleId = poetSampleId
        };

        await _unitOfWork.GetRepository<PoetSampleTitleSample>().InsertAsync(poetSampleTitleSample);
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

        /*// Add title sample
        if (request.TitleSampleIds != null)
        {
            foreach (var titleSampleId in request.TitleSampleIds)
            {
                await AddPoetSampleTitleSample(existPoetSample.Id, titleSampleId);
            }
        }*/

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
            if (request.FilterOptions.Name != null)
            {
                poetSampleQuery = poetSampleQuery.Where(p =>
                    p.Name.ToLower().Trim().Contains(request.FilterOptions.Name.ToLower().Trim()));
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

        IList<GetPoetSampleResponse> poetSamples = new List<GetPoetSampleResponse>();
        foreach (var poetSample in queryPaging.Data)
        {
            PoetSample? poetSampleEntity = await _unitOfWork.GetRepository<PoetSample>()
                .FindAsync(p => p.Id == poetSample.Id && p.DeletedTime == null);
            if (poetSampleEntity == null)
            {
                continue;
            }

            poetSamples.Add(_mapper.Map<GetPoetSampleResponse>(poetSampleEntity));

            // Map title samples

            if (poetSampleEntity.PoetSampleTitleSamples != null)
            {
                poetSamples.Last().TitleSamples = _mapper.Map<IList<GetTitleSampleResponse>>(
                    poetSampleEntity.PoetSampleTitleSamples.Select(p => p.TitleSample));
            }
        }
        //var poetSamples = _mapper.Map<IList<GetPoetSampleResponse>>(queryPaging.Data);

        return new PaginationResponse<GetPoetSampleResponse>(poetSamples, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }

    public async Task<GetPoetSampleResponse> GetPoetSample(Guid poetSampleId)
    {
        PoetSample? poetSample = await _unitOfWork.GetRepository<PoetSample>()
            .FindAsync(p => p.Id == poetSampleId && p.DeletedTime == null);

        // Check if the poet sample exists
        if (poetSample == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "PoetSample is not exist");
        }

        var poetSampleMapping = _mapper.Map<GetPoetSampleResponse>(poetSample);

        // Map title samples
        if (poetSample.PoetSampleTitleSamples != null)
        {
            poetSampleMapping.TitleSamples = _mapper.Map<IList<GetTitleSampleResponse>>(
                poetSample.PoetSampleTitleSamples.Select(p => p.TitleSample));
        }

        return poetSampleMapping;
    }

    /*public async Task RemovePoetSampleTitleSample(Guid poetSampleId, IList<Guid> titleSampleIds)
    {
        var poetSampleTitleSamples = await _unitOfWork.GetRepository<PoetSampleTitleSample>()
            .AsQueryable()
            .Where(p => titleSampleIds.Contains(p.TitleSampleId) && p.PoetSampleId == poetSampleId)
            .ToListAsync();
        
        // Check if the poet sample title sample exists
        if(poetSampleTitleSamples == null || poetSampleTitleSamples.Count == 0)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "PoetSampleTitleSample is not exist");
        }

        _unitOfWork.GetRepository<PoetSampleTitleSample>().DeletePermanentRange(poetSampleTitleSamples);
        await _unitOfWork.SaveChangesAsync();
    }*/
    
    public async Task<IList<GetPoetSampleResponse>> GetLiveBoardPoetSamples()
    {
        var poetSamples = await _unitOfWork.GetRepository<PoetSample>()
            .AsQueryable()
            .Where(p => p.DeletedTime == null)
            .OrderByDescending(p => p.CreatedTime)
            .ToListAsync();
        
        return _mapper.Map<IList<GetPoetSampleResponse>>(poetSamples);
    }
}