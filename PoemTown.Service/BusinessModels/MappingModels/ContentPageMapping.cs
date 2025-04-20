using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.ContentPageRequests;
using PoemTown.Service.BusinessModels.ResponseModels.ContenPageResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class ContentPageMapping : Profile
{
    public ContentPageMapping()
    {
        CreateMap<CreateNewContentPageRequest, ContentPage>();
        CreateMap<UpdateContentPageRequest, ContentPage>();

        CreateMap<ContentPage, GetContentPageResponse>().ReverseMap();
    }
}