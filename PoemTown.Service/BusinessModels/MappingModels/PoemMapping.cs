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
        CreateMap<Poem, PoemPlagiarismFromResponse>().ReverseMap();
        CreateMap<CreateNewPoemRequest, Poem>();
        CreateMap<UpdatePoemRequest, Poem>();
        CreateMap<Poem, GetPoemInOrderDetailResponse>().ReverseMap();
        CreateMap<CreatePoetSamplePoemRequest, Poem>();
        CreateMap<Poem, GetPoemInReportResponse>().ReverseMap();
        CreateMap<Poem, GetPoetSamplePoemResponse>().ReverseMap();
        CreateMap<UpdatePoetSamplePoemRequest, Poem>();
        
        CreateMap<GetPoemResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count))
            .ForMember(dest => dest.IsSellUsageRight, opt => opt.MapFrom(p => p.SaleVersions!.Any(sv => sv.Status == SaleVersionStatus.InSale)))
            .ForMember(dest => dest.SaleVersion, opt => opt.MapFrom(p => p.SaleVersions!.FirstOrDefault(sv => sv.IsInUse == true)));
            /*.ForMember(dest => dest.User,
                opt => opt.MapFrom(src =>
                    // Pick the User from the UserPoemRecordFiles that is the copyright holder.
                    src.UserPoemRecordFiles.FirstOrDefault(uprf => uprf.Type == UserPoemType.CopyRightHolder).User))*/

        CreateMap<GetRecordFileResponse, GetPoemDetailResponse>().ReverseMap();
        CreateMap<GetPoemDetailResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.RecordFiles, opt => opt.Ignore())
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count))
            //.ForMember(dest => dest.IsSellUsageRight, opt => opt.MapFrom(p => p.SaleVersions!.Any(sv => sv.Status == SaleVersionStatus.InSale)))
            .ForMember(dest => dest.SaleVersion, opt => opt.MapFrom(p => p.SaleVersions!.FirstOrDefault(sv => sv.IsInUse == true)));
        
        CreateMap<GetPoemInCollectionResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count))
            .ForMember(dest => dest.IsSellUsageRight, opt => opt.MapFrom(p => p.SaleVersions!.Any(sv => sv.Status == SaleVersionStatus.InSale)))
            .ForMember(dest => dest.SaleVersion, opt => opt.MapFrom(p => p.SaleVersions!.FirstOrDefault(sv => sv.IsInUse == true)));

        CreateMap<GetPostedPoemResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count))
            .ForMember(dest => dest.IsSellUsageRight, opt => opt.MapFrom(p => p.SaleVersions!.Any(sv => sv.Status == SaleVersionStatus.InSale)))
            .ForMember(dest => dest.SaleVersion, opt => opt.MapFrom(p => p.SaleVersions!.FirstOrDefault(sv => sv.IsInUse == true)));

        CreateMap<GetUserPoemResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count))
            .ForMember(dest => dest.IsSellUsageRight, opt => opt.MapFrom(p => p.SaleVersions!.Any(sv => sv.Status == SaleVersionStatus.InSale)))
            .ForMember(dest => dest.SaleVersion, opt => opt.MapFrom(p => p.SaleVersions!.FirstOrDefault(sv => sv.IsInUse == true))); 
        
        // poem in target mark
        CreateMap<GetPoemInTargetMarkResponse, Poem>().ReverseMap()
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(p => p.Likes!.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(p => p.Comments!.Count))
            .ForMember(dest => dest.IsSellUsageRight, opt => opt.MapFrom(p => p.SaleVersions!.Any(sv => sv.Status == SaleVersionStatus.InSale)))
            .ForMember(dest => dest.SaleVersion, opt => opt.MapFrom(p => p.SaleVersions!.FirstOrDefault(sv => sv.IsInUse == true)));


        // poem in collection
        CreateMap<GetCollectionInPoemResponse, GetPoemResponse>();
        CreateMap<GetCollectionInPoemResponse, GetPoemDetailResponse>();
        CreateMap<GetCollectionInPoemResponse, GetPoemResponse>();
        CreateMap<GetCollectionInPoemResponse, GetPoetSamplePoemResponse>();
    }
}