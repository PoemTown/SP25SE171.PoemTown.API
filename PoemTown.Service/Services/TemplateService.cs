using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.TemplateDetails;
using PoemTown.Repository.Enums.Templates;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;
using PoemTown.Service.BusinessModels.ResponseModels.TemplateResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.TemplateFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.TemplateSorts;
using PoemTown.Service.ThirdParties.Interfaces;

namespace PoemTown.Service.Services;

public class TemplateService : ITemplateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAwsS3Service _awsS3Service;

    public TemplateService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateMasterTemplate(CreateMasterTemplateRequest request)
    {
        var masterTemplate = _mapper.Map<MasterTemplate>(request);

        // Check if MasterTemplateDetails is null then insert MasterTemplate only
        if (request.MasterTemplateDetails == null)
        {
            await _unitOfWork.GetRepository<MasterTemplate>().InsertAsync(masterTemplate);
            await _unitOfWork.SaveChangesAsync();
            return;
        }

        /*// Check if MasterTemplateDetails Type is duplicate
        var duplicateMasterTemplateDetailType = request.MasterTemplateDetails
            .GroupBy(x => x.Type)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .FirstOrDefault();

        if (duplicateMasterTemplateDetailType != default)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, $"Duplicate MasterTemplateDetail Type: {duplicateMasterTemplateDetailType}");
        }*/

        // Set MasterTemplate Type based on MasterTemplateDetails count: Single or Bundle (>1)
        masterTemplate.Type = request.MasterTemplateDetails.Count > 1 ? TemplateType.Bundle : TemplateType.Single;
        await _unitOfWork.GetRepository<MasterTemplate>().InsertAsync(masterTemplate);
        foreach (var mtd in request.MasterTemplateDetails)
        {
            if (mtd is { Image: not null, ColorCode: not null })
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Image and ColorCode cannot be together");
            }

            if (mtd is { Image: null, ColorCode: null })
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Image or ColorCode is required");
            }

            MasterTemplateDetail masterTemplateDetail = _mapper.Map<MasterTemplateDetail>(mtd);

            masterTemplateDetail.MasterTemplateId = masterTemplate.Id;
            // Set DesignType based on Image or ColorCode
            masterTemplateDetail.DesignType =
                mtd.Image != null ? TemplateDetailDesignType.Image : TemplateDetailDesignType.ColorCode;

            await _unitOfWork.GetRepository<MasterTemplateDetail>().InsertAsync(masterTemplateDetail);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PaginationResponse<GetMasterTemplateResponse>> GetMasterTemplate
        (RequestOptionsBase<GetMasterTemplateFilterOption, GetMasterTemplateSortOption> request)
    {
        var masterTemplateQuery = _unitOfWork.GetRepository<MasterTemplate>().AsQueryable();

        if (request.IsDelete == true)
        {
            masterTemplateQuery = masterTemplateQuery.Where(p => p.DeletedTime != null);
        }
        else
        {
            masterTemplateQuery = masterTemplateQuery.Where(p => p.DeletedTime == null);
        }

        // Filter
        if (request.FilterOptions != null)
        {
            if (!string.IsNullOrWhiteSpace(request.FilterOptions.TemplateName))
            {
                masterTemplateQuery = masterTemplateQuery
                    .Where(p => p.TemplateName!.Equals(request.FilterOptions.TemplateName,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (request.FilterOptions.Type != null)
            {
                masterTemplateQuery = masterTemplateQuery.Where(p => p.Type == request.FilterOptions.Type);
            }

            if (request.FilterOptions.PriceRange != null)
            {
                masterTemplateQuery = masterTemplateQuery.Where(p =>
                    p.Price >= request.FilterOptions.PriceRange.PriceFrom
                    && p.Price <= request.FilterOptions.PriceRange.PriceTo);
            }

            if (!string.IsNullOrWhiteSpace(request.FilterOptions.TagName))
            {
                masterTemplateQuery = masterTemplateQuery
                    .Where(p => p.TagName!.Equals(request.FilterOptions.TagName, StringComparison.OrdinalIgnoreCase));
            }
        }

        // Sort
        masterTemplateQuery = request.SortOptions switch
        {
            GetMasterTemplateSortOption.CreatedTimeAscending => masterTemplateQuery.OrderBy(p => p.CreatedTime),
            GetMasterTemplateSortOption.CreatedTimeDescending => masterTemplateQuery.OrderByDescending(p =>
                p.CreatedTime),
            GetMasterTemplateSortOption.PriceAscending => masterTemplateQuery.OrderBy(p => p.Price),
            GetMasterTemplateSortOption.PriceDescending => masterTemplateQuery.OrderByDescending(p => p.Price),
            _ => masterTemplateQuery.OrderByDescending(p => p.CreatedTime)
        };

        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<MasterTemplate>()
            .GetPagination(masterTemplateQuery, request.PageNumber, request.PageSize);

        var masterTemplateResponse = _mapper.Map<IList<GetMasterTemplateResponse>>(queryPaging.Data);

        return new PaginationResponse<GetMasterTemplateResponse>(masterTemplateResponse, queryPaging.PageNumber,
            queryPaging.PageSize, queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }

    public async Task<PaginationResponse<GetMasterTemplateDetailResponse>> GetMasterTemplateDetail
    (Guid masterTemplateId,
        RequestOptionsBase<GetMasterTemplateDetailFilterOption, GetMasterTemplateDetailSortOption> request)
    {
        // Check if MasterTemplate exists
        MasterTemplate? masterTemplate =
            await _unitOfWork.GetRepository<MasterTemplate>().FindAsync(p => p.Id == masterTemplateId);
        if (masterTemplate == null)
        {
            throw new CoreException(StatusCodes.Status404NotFound, "MasterTemplate not found");
        }

        var masterTemplateDetailQuery = _unitOfWork.GetRepository<MasterTemplateDetail>().AsQueryable();
        masterTemplateDetailQuery = masterTemplateDetailQuery.Where(p => p.MasterTemplateId == masterTemplateId);
        
        
        if (request.IsDelete == true)
        {
            masterTemplateDetailQuery = masterTemplateDetailQuery.Where(p => p.DeletedTime != null);
        }
        else
        {
            masterTemplateDetailQuery = masterTemplateDetailQuery.Where(p => p.DeletedTime == null);
        }
        // Filter
        if (request.FilterOptions != null)
        {
        }

        // Sort
        masterTemplateDetailQuery = request.SortOptions switch
        {
            GetMasterTemplateDetailSortOption.CreatedTimeAscending => masterTemplateDetailQuery.OrderBy(p =>
                p.CreatedTime),
            GetMasterTemplateDetailSortOption.CreatedTimeDescending => masterTemplateDetailQuery.OrderByDescending(p =>
                p.CreatedTime),
            _ => masterTemplateDetailQuery.OrderByDescending(p => p.CreatedTime)
        };

        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<MasterTemplateDetail>()
            .GetPagination(masterTemplateDetailQuery, request.PageNumber, request.PageSize);

        var masterTemplateDetailResponse = _mapper.Map<IList<GetMasterTemplateDetailResponse>>(queryPaging.Data);

        return new PaginationResponse<GetMasterTemplateDetailResponse>(masterTemplateDetailResponse,
            queryPaging.PageNumber,
            queryPaging.PageSize, queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }

    public async Task UpdateMasterTemplate(UpdateMasterTemplateRequest request)
    {
        var masterTemplate = await _unitOfWork.GetRepository<MasterTemplate>().FindAsync(p => p.Id == request.Id);
        if (masterTemplate == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "MasterTemplate not found");
        }

        _mapper.Map(request, masterTemplate);
        _unitOfWork.GetRepository<MasterTemplate>().Update(masterTemplate);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateMasterTemplateDetail(UpdateMasterTemplateDetailRequest request)
    {
        var masterTemplateDetail =
            await _unitOfWork.GetRepository<MasterTemplateDetail>().FindAsync(p => p.Id == request.Id);

        if (masterTemplateDetail == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "MasterTemplateDetail not found");
        }

        if (request is { ColorCode: not null } && request is { Image: not null })
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Image and ColorCode cannot be together");
        }

        switch (masterTemplateDetail.DesignType)
        {
            case TemplateDetailDesignType.Image:
                if (request is { Image: null })
                {
                    throw new CoreException(StatusCodes.Status400BadRequest,
                        "This template detail is using Image => Image is required");
                }

                break;
            case TemplateDetailDesignType.ColorCode:
                if (request is { ColorCode: null })
                {
                    throw new CoreException(StatusCodes.Status400BadRequest,
                        "This template detail is using ColorCode => ColorCode is required");
                }

                break;
        }

        _mapper.Map(request, masterTemplateDetail);
        _unitOfWork.GetRepository<MasterTemplateDetail>().Update(masterTemplateDetail);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteMasterTemplate(Guid masterTemplateId)
    {
        var masterTemplate = await _unitOfWork.GetRepository<MasterTemplate>().FindAsync(p => p.Id == masterTemplateId);
        if (masterTemplate == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "MasterTemplate not found");
        }

        _unitOfWork.GetRepository<MasterTemplate>().Delete(masterTemplate);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteMasterTemplateDetail(Guid masterTemplateDetailId)
    {
        var masterTemplateDetail = await _unitOfWork.GetRepository<MasterTemplateDetail>()
            .FindAsync(p => p.Id == masterTemplateDetailId);
        if (masterTemplateDetail == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "MasterTemplateDetail not found");
        }

        _unitOfWork.GetRepository<MasterTemplateDetail>().Delete(masterTemplateDetail);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteMasterTemplatePermanently(Guid masterTemplateId)
    {
        var masterTemplate = await _unitOfWork.GetRepository<MasterTemplate>().FindAsync(p => p.Id == masterTemplateId);
        if (masterTemplate == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "MasterTemplate not found");
        }

        if (masterTemplate.DeletedTime == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "MasterTemplate has not yet soft deleted");
        }

        var masterTemplateDetails = await _unitOfWork.GetRepository<MasterTemplateDetail>()
            .AsQueryable()
            .Where(p => p.MasterTemplateId == masterTemplateId)
            .ToListAsync();

        foreach (var masterTemplateDetail in masterTemplateDetails)
        {
            _unitOfWork.GetRepository<MasterTemplateDetail>().DeletePermanent(masterTemplateDetail);
        }

        _unitOfWork.GetRepository<MasterTemplate>().DeletePermanent(masterTemplate);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteMasterTemplateDetailPermanently(Guid masterTemplateDetailId)
    {
        var masterTemplateDetail = await _unitOfWork.GetRepository<MasterTemplateDetail>()
            .FindAsync(p => p.Id == masterTemplateDetailId);
        if (masterTemplateDetail == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "MasterTemplateDetail not found");
        }

        if (masterTemplateDetail.DeletedTime == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "MasterTemplateDetail has not yet soft deleted");
        }

        _unitOfWork.GetRepository<MasterTemplateDetail>().DeletePermanent(masterTemplateDetail);
        await _unitOfWork.SaveChangesAsync();
    }
}