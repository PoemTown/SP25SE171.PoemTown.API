using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.TemplateRequests;
using PoemTown.Service.BusinessModels.ResponseModels.TemplateResponses;
using PoemTown.Service.QueryOptions.FilterOptions.TemplateFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.TemplateSorts;

namespace PoemTown.Service.Interfaces;

public interface ITemplateService
{
    Task CreateMasterTemplate(CreateMasterTemplateRequest request);

    Task<PaginationResponse<GetMasterTemplateResponse>> GetMasterTemplate
        (RequestOptionsBase<GetMasterTemplateFilterOption, GetMasterTemplateSortOption> request);

    Task<PaginationResponse<GetMasterTemplateDetailResponse>> GetMasterTemplateDetail
    (Guid masterTemplateId,
        RequestOptionsBase<GetMasterTemplateDetailFilterOption, GetMasterTemplateDetailSortOption> request);

    Task UpdateMasterTemplate(UpdateMasterTemplateRequest request);
    Task UpdateMasterTemplateDetail(UpdateMasterTemplateDetailRequest request);
    Task DeleteMasterTemplate(Guid masterTemplateId);
    Task DeleteMasterTemplatePermanently(Guid masterTemplateId);
    Task DeleteMasterTemplateDetail(Guid masterTemplateDetailId);
    Task DeleteMasterTemplateDetailPermanently(Guid masterTemplateDetailId);
}