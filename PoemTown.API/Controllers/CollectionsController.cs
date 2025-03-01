using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.CollectionRequest;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.CollectionFilters;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.CollectionSorts;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;
using PoemTown.Service.Services;
using System.Security.Claims;

namespace PoemTown.API.Controllers
{
    public class CollectionsController : BaseController
    {
        private readonly ICollectionService _service;
        private readonly IMapper _mapper;

        public CollectionsController(IMapper mapper, ICollectionService service)
        {
            _mapper = mapper;
            _service = service;
        }

        /// <summary>
        /// Tạo mới một bộ sưu tập, yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// 
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> CreateCollection(CreateCollectionRequest request)
        {
            string role = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value;
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            await _service.CreateCollection(userId, request, role);
            return Ok(new BaseResponse(StatusCodes.Status201Created, "Collection created successfully"));
        }
        /// <summary>
        /// Cập nhật bộ sưu tập, yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// 
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("v1")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> UpdateCollection(UpdateCollectionRequest request)
        {
            await _service.UpdateCollection(request);
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Collection updated successfully"));
        }
        /// <summary>
        /// Lấy danh sách tất cả bộ sưu tập
        /// </summary>
        /// <remarks>
        /// SortOptions: Sắp xếp bộ sưu tập theo thứ tự
        ///
        /// - 1: CreateTimeAscending (Thời gian cũ đến mới)
        /// - 2: CreateTimeDescending (Thời gian mới đến cũ)
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <param name="targetUserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1")]
        [Authorize]
        public async Task<ActionResult<BasePaginationResponse<GetCollectionResponse>>>
        GetCollections(RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request, [FromQuery] Guid? targetUserId = null)
        {
            if (targetUserId == null)
            {
                targetUserId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            }
            var result = await _service.GetCollections(targetUserId.Value, request);
            var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetCollectionResponse>>(result);
            basePaginationResponse.StatusCode = StatusCodes.Status200OK;
            basePaginationResponse.Message = "Get Collection successfully";
            return Ok(basePaginationResponse);
        }
        /// <summary>
        /// Xóa bộ sưu tập (Chuyển vào thùng rác), yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// 
        ///
        /// </remarks>
        /// <param name="collectionId"></param>
        /// <param name="rowVersion"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("v1/{collectionId}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> DeletePoem(Guid collectionId, [FromQuery] byte[] rowVersion)
        {
            await _service.DeleteCollection(collectionId, rowVersion);
            return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Collection deleted successfully"));
        }
        /// <summary>
        /// Xóa bộ sưu tập (Vĩnh viễn), yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// 
        ///
        /// </remarks>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("v1/{collectionId}/permanent")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> DeletePoemPermanent(Guid collectionId)
        {
            await _service.DeleteCollectionPermanent(collectionId);
            return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Collection deleted permanent successfully"));
        }
        /// <summary>
        /// Thêm một bài thơ vào bộ sưu tập, yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// 
        ///
        /// </remarks>
        /// <param name="collectionId"></param>
        /// <param name="poemId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/{collectionId}/{poemId}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> AddPoemToCollection(Guid collectionId, Guid poemId)
        {
            await _service.AddPoemToCollection(poemId, collectionId);
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Add poem to collection successfully"));
        }
        /// <summary>
        /// Lấy danh sách tất cả bộ sưu tập phổ biến, không yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        ///
        /// 
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/trending")]
        public async Task<ActionResult<BasePaginationResponse<GetCollectionResponse>>> GetTrendingCollections(RequestOptionsBase<CollectionFilterOption, CollectionSortOptions> request)
        {
            var userClaim = User.Claims.FirstOrDefault(p => p.Type == "UserId");
            Guid? userId = null;
            if (userClaim != null)
            {
                userId = Guid.Parse(userClaim.Value);
            }        
            var result = await _service.GetTrendingCollections(userId, request);
            var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetCollectionResponse>>(result);
            basePaginationResponse.StatusCode = StatusCodes.Status200OK;
            basePaginationResponse.Message = "Get Collection successfully";
            return Ok(basePaginationResponse);
        }



        /// <summary>
        /// Lấy chi tiết của một bộ sưu tập, yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        ///
        /// - collectionId: lấy từ request path
        /// </remarks>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/{collectionId}/detail")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<GetPoemDetailResponse>>>
            GetPoemDetail(Guid collectionId)
        {
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            var response = await _service.GetCollectionDetail(collectionId, userId);
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get collection detail successfully", response));
        }


        /// <summary>
        /// Upload ảnh của bộ sưu tập, yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="file">lấy từ form data</param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/image")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<string>>> UploadCollectionImage(IFormFile file)
        {
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            var response = await _service.UploadProfileImage(userId, file);
            return Ok(new BaseResponse<string>(StatusCodes.Status201Created, "Profile image uploaded successfully", response));
        }
    }
}
