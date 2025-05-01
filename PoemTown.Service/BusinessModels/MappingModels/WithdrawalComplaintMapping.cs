using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.WithdrawalComplaints;
using PoemTown.Service.BusinessModels.RequestModels.WithdrawalComplaintRequests;
using PoemTown.Service.BusinessModels.RequestModels.WithdrawalFormRequests;
using PoemTown.Service.BusinessModels.ResponseModels.WithdrawalComplaintResponses;
using SixLabors.ImageSharp.ColorSpaces.Companding;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class WithdrawalComplaintMapping : Profile
{
    public WithdrawalComplaintMapping()
    {
        CreateMap<CreateNewWithdrawalComplaintRequest, WithdrawalComplaint>()
            .ForMember(p => p.Images, opt => opt.Ignore());
        CreateMap<UpdateWithdrawalComplaintRequest, WithdrawalComplaint>()
            .ForMember(p => p.Images, opt => opt.Ignore());

        CreateMap<WithdrawalComplaint, GetWithdrawalComplaintResponse>()
            .ForMember(dest => dest.ComplaintImages, opt => opt.MapFrom(src =>
                src.Images
                    .Where(img => img.Type == WithdrawalComplaintType.Complaint)))
            .ForMember(dest => dest.ResolveImages, opt => opt.MapFrom(src =>
                src.Images
                    .Where(img => img.Type == WithdrawalComplaintType.ResolveComplaint)));
        CreateMap<WithdrawalComplaintImage, GetWithdrawalComplaintImageResponse>().ReverseMap();
        /*CreateMap<List<GetWithdrawalComplaintImageResponse>, GetWithdrawalComplaintResponse>()
            .ForMember(dest => dest.ComplaintImages, opt => opt.MapFrom(src => src.Where(p => p.Type == WithdrawalComplaintType.Complaint).ToList()))
            .ForMember(dest => dest.ResolveImages, opt => opt.MapFrom(src => src.Where(p => p.Type == WithdrawalComplaintType.ResolveComplaint).ToList()));*/
    }
}