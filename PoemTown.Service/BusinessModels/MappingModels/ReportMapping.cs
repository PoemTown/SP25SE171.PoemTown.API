using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.ReportResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class ReportMapping : Profile
{
    public ReportMapping()
    {
        CreateMap<Report, GetReportResponse>().ReverseMap();
        CreateMap<Report, GetMyReportResponse>().ReverseMap();

        CreateMap<ReportMessage, GetReportMessageResponse>().ReverseMap();

        CreateMap<GetReportMessageResponse, GetMyReportResponse>();
        CreateMap<GetReportMessageResponse, GetReportResponse>();
    }
}