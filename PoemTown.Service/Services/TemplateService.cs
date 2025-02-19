using AutoMapper;
using Microsoft.AspNetCore.Http;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Templates;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;
using PoemTown.Service.Interfaces;
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
            
            await _unitOfWork.GetRepository<MasterTemplateDetail>().InsertAsync(masterTemplateDetail);
        }
        await _unitOfWork.SaveChangesAsync();
    }
}