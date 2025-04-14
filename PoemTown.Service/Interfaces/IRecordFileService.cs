using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.RecordFileRequests;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Interfaces
{
    public interface IRecordFileService
    {
        Task CreateNewRecord(Guid userId, Guid poemID, CreateNewRecordFileRequest request);
        Task UpdateNewRecord(Guid userId, UpdateRecordRequest request);
        Task DeleteNewRecord(Guid userId, Guid recordId);
        Task AssigntToPrivate(Guid userId, AssignPrivateRequest request);
        Task PurchaseRecordFile(Guid userId, Guid recordId);
        Task<PaginationResponse<GetSoldRecordResponse>>GetSoldRecord(Guid? userId, RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request);
        Task<PaginationResponse<GetBoughtRecordResponse>> GetBoughtRecord(Guid? userId, RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request);
        Task<PaginationResponse<GetRecordFileResponse>> GetAllRecord(Guid? userId, RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request);
        Task<PaginationResponse<GetRecordFileResponse>> GetUserRecord(string username,RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request);
        Task<PaginationResponse<GetRecordFileResponse>> GetMyRecord(Guid? userId, RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request);
        Task<string> UploadRecordFile(Guid userId, IFormFile file);
        Task<IActionResult> GetAudioStreamResultAsync(Guid id);
        Task<GetRecordFileResponse> GetRecordDetail(Guid recordId);
        Task CountRecordView(Guid recordId, int duration);
    }
}
