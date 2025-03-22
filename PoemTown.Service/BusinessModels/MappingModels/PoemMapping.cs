using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.SaleVersions;
using PoemTown.Repository.Enums.UserPoems;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.OrderResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemHistoryResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;
using PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class PoemMapping : Profile
{
    public PoemMapping()
    {
        CreateMap<CreateNewPoemRequest, Poem>();
        CreateMap<UpdatePoemRequest, Poem>();
        CreateMap<Poem, GetPoemInOrderDetailResponse>().ReverseMap();

        CreateMap<Poem, GetPoemInReportResponse>().ReverseMap();
        
        CreateMap<GetPoemResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count))
            .ForMember(dest => dest.IsSellCopyRight, opt => opt.MapFrom(p => p.SaleVersions!.Any(sv => sv.Status == SaleVersionStatus.InSale)))
            /*.ForMember(dest => dest.User,
                opt => opt.MapFrom(src =>
                    // Pick the User from the UserPoemRecordFiles that is the copyright holder.
                    src.UserPoemRecordFiles.FirstOrDefault(uprf => uprf.Type == UserPoemType.CopyRightHolder).User))*/;

        CreateMap<GetRecordFileResponse, GetPoemDetailResponse>().ReverseMap();
        CreateMap<GetPoemDetailResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.RecordFiles, opt => opt.Ignore())
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count))
            .ForMember(dest => dest.IsSellCopyRight, opt => opt.MapFrom(p => p.SaleVersions!.Any(sv => sv.Status == SaleVersionStatus.InSale )));
        
        CreateMap<GetPoemInCollectionResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count))
            .ForMember(dest => dest.IsSellCopyRight, opt => opt.MapFrom(p => p.SaleVersions!.Any(sv => sv.Status == SaleVersionStatus.InSale)));
        CreateMap<GetPostedPoemResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count))
            .ForMember(dest => dest.IsSellCopyRight, opt => opt.MapFrom(p => p.SaleVersions!.Any(sv => sv.Status == SaleVersionStatus.InSale)));
        
        // poem in target mark
        CreateMap<GetPoemInTargetMarkResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count))
            .ForMember(dest => dest.IsSellCopyRight, opt => opt.MapFrom(p => p.SaleVersions!.Any(sv => sv.Status == SaleVersionStatus.InSale)));

        // poem in collection
        CreateMap<GetCollectionInPoemResponse, GetPoemResponse>();
        CreateMap<GetCollectionInPoemResponse, GetPoemDetailResponse>();
        CreateMap<GetCollectionInPoemResponse, GetPoemResponse>();
    }
}