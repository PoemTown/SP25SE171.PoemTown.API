using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.UsageResponse;
using PoemTown.Service.QueryOptions.FilterOptions.UsageRightFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.UsageRightSorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Interfaces
{
    public interface IUsageRightService
    {
        Task TimeOutUsageRight();
        Task RenewLicense(Guid usageRightId);
        Task<PaginationResponse<GetSoldPoemResponse>> GetSoldPoem(Guid userId, RequestOptionsBase<GetUsageRightPoemFilter, GetUsageRightPoemSort> request);
        Task<PaginationResponse<GetBoughtPoemResponse>> GetBoughtPoem(Guid userId, RequestOptionsBase<GetUsageRightPoemFilter, GetUsageRightPoemSort> request);
        Task<PaginationResponse<GetPoemVersionResponse>> VersionByPoemId(Guid userId, Guid poemId, RequestOptionsBase<object, object> request);
    }
}
