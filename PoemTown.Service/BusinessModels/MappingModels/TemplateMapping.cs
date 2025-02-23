using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;
using PoemTown.Service.BusinessModels.ResponseModels.TemplateResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class TemplateMapping : Profile
{
    public TemplateMapping()
    {
        // MasterTemplate
        CreateMap<CreateMasterTemplateRequest, MasterTemplate>()
            .ForMember(p => p.MasterTemplateDetails, opt => opt.Ignore());
        CreateMap<MasterTemplate, GetMasterTemplateResponse>().ReverseMap();
        CreateMap<UpdateMasterTemplateRequest, MasterTemplate>();
        
        // MasterTemplateDetail
        CreateMap<CreateMasterTemplateDetailRequest, MasterTemplateDetail>();
        CreateMap<MasterTemplateDetail, GetMasterTemplateDetailResponse>().ReverseMap();
        CreateMap<UpdateMasterTemplateDetailRequest, MasterTemplateDetail>();
    }
}