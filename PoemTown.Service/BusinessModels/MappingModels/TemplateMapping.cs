using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class TemplateMapping : Profile
{
    public TemplateMapping()
    {
        // MasterTemplate
        CreateMap<CreateMasterTemplateRequest, MasterTemplate>()
            .ForMember(p => p.MasterTemplateDetails, opt => opt.Ignore());
        
        // MasterTemplateDetail
        CreateMap<CreateMasterTemplateDetailRequest, MasterTemplateDetail>();
    }
}