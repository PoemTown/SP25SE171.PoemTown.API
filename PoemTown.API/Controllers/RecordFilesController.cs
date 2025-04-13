using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.RequestModels.RecordFileRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;
using PoemTown.Service.Services;

namespace PoemTown.API.Controllers
{
    public class RecordFilesController : BaseController
    {
        private readonly IRecordFileService _service;
        private readonly IMapper _mapper;
        public RecordFilesController(IRecordFileService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Tạo mới một bảng ghi âm giọng bình thơ, yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        ///
        /// 
        ///
        /// </remarks>
        /// <param name="poemId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> CreateNewPoem([FromQuery]Guid poemId, CreateNewRecordFileRequest request)
        {
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            await _service.CreateNewRecord(userId, poemId, request);
            return Ok(new BaseResponse(StatusCodes.Status201Created, "Record file created successfully"));
        }

        /// <summary>
        /// Chỉnh sửa một bản ghi âm, yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// Nếu đã bán thì không update được
        /// </remarks>

        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("v1")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> UpdatePoem(UpdateRecordRequest request)
        {
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            await _service.UpdateNewRecord(userId, request);
            return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Record file updated successfully"));
        }

        /// <summary>
        /// Xóa một bản ghi âm (Chuyển vào thùng rác), yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// CHÚ Ý REQUEST PARAMETER:
        ///
        /// - recordId: lấy từ request path
        /// </remarks>
        /// <param name="recordId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("v1/{recordId}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> DeletePoem(Guid recordId)
        {
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            await _service.DeleteNewRecord(userId, recordId);
            return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Poem deleted successfully"));
        }



        /// <summary>
        /// Lấy danh sách bản ngâm thơ đã bán, yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/sold")]
        public async Task<ActionResult<BaseResponse<GetSoldRecordResponse>>> GetSoldRecord(RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
        {
            var userClaim = User.Claims.FirstOrDefault(p => p.Type == "UserId");
            Guid? userId = null;
            if (userClaim != null)
            {
                userId = Guid.Parse(userClaim.Value);
            }
            var paginationResponse = await _service.GetSoldRecord(userId, request);
            var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetSoldRecordResponse>>(paginationResponse);
            basePaginationResponse.StatusCode = StatusCodes.Status200OK;
            basePaginationResponse.Message = "Get sold record successfully";

            return Ok(basePaginationResponse);
        }

        /// <summary>
        /// Lấy danh sách bản ngâm thơ đã mua, yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/bought")]
        public async Task<ActionResult<BaseResponse<GetBoughtRecordResponse>>> GetBoughtRecord(RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
        {
            var userClaim = User.Claims.FirstOrDefault(p => p.Type == "UserId");
            Guid? userId = null;
            if (userClaim != null)
            {
                userId = Guid.Parse(userClaim.Value);
            }
            var paginationResponse = await _service.GetBoughtRecord(userId, request);
            var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetBoughtRecordResponse>>(paginationResponse);
            basePaginationResponse.StatusCode = StatusCodes.Status200OK;
            basePaginationResponse.Message = "Get bought record successfully";

            return Ok(basePaginationResponse);
        }
        /// <summary>
        /// Lấy danh sách bản ngâm thơ cua toi, yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/mine")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<GetRecordFileResponse>>> GetMyRecord(RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
        {
            var userClaim = User.Claims.FirstOrDefault(p => p.Type == "UserId");
            Guid? userId = null;
            if (userClaim != null)
            {
                userId = Guid.Parse(userClaim.Value);
            }

            var paginationResponse = await _service.GetMyRecord(userId, request);
            var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetRecordFileResponse>>(paginationResponse);
            basePaginationResponse.StatusCode = StatusCodes.Status200OK;
            basePaginationResponse.Message = "Get my record successfully";

            return Ok(basePaginationResponse);
        }

        /// <summary>
        /// Lấy danh sách bản ngâm thơ của người dùng, không yêu cầu đăng nhập 
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/user/{userName}")]
        public async Task<ActionResult<BaseResponse<GetRecordFileResponse>>> GetUserRecord(string userName, RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
        {
            var paginationResponse = await _service.GetUserRecord(userName, request);
            var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetRecordFileResponse>>(paginationResponse);
            basePaginationResponse.StatusCode = StatusCodes.Status200OK;
            basePaginationResponse.Message = "Get my record successfully";

            return Ok(basePaginationResponse);
        }

        /// <summary>
        /// Lấy danh sách bản ngâm thơ public, không yêu cầu đăng nhập 
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/all")]
        public async Task<ActionResult<BaseResponse<GetRecordFileResponse>>> GetAllRecord(RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
        {
            var userClaim = User.Claims.FirstOrDefault(p => p.Type == "UserId");
            Guid? userId = null;
            if (userClaim != null)
            {
                userId = Guid.Parse(userClaim.Value);
            }
            var paginationResponse = await _service.GetAllRecord(userId, request);
            var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetRecordFileResponse>>(paginationResponse);
            basePaginationResponse.StatusCode = StatusCodes.Status200OK;
            basePaginationResponse.Message = "Get all record successfully";

            return Ok(basePaginationResponse);
        }
        /// <summary>
        /// Chuyển bản ngâm thơ thành riêng tư, yêu cầu đăng nhập
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("v1/enable-selling")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> AssigntRecordToPrivate(AssignPrivateRequest request)
        {
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            await _service.AssigntToPrivate(userId, request);
            return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Enable record file to private successfully"));
        }

        /// <summary>
        /// Mua bản quyền của một bản ngâm thơ, yêu cầu đăng nhập
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("v1/purchase")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> PurchaseRecord([FromQuery] Guid recordId)
        {
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            await _service.PurchaseRecordFile(userId, recordId);
            return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Purchase record file successfully"));
        }


        /// <summary>
        /// Upload file ghi âm cho bài thơ, yêu cầu đăng nhập
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/audio")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<string>>> UploadPoemAudio(IFormFile file)
        {
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            var response = await _service.UploadRecordFile(userId, file);
            return Ok(new BaseResponse<string>(StatusCodes.Status201Created, "Upload audio successfully", response));
        }



        [HttpGet]
        [Route("v1/audio-stream/{id}")]
        [Authorize]
        public async Task<IActionResult> StreamAudio(Guid id)
        {

            var fileStreamResult = await _service.GetAudioStreamResultAsync(id);
            if (fileStreamResult == null)
            {
                return NotFound("Không tìm thấy file audio.");
            }
            return fileStreamResult;

        }

        /// <summary>
        /// Lấy chi tiết của một bản ghi âm, không yêu cầu đăng nhập
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <param name="recordId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/{recordId}/detail")]
        public async Task<ActionResult<BaseResponse<GetRecordFileResponse>>>
            GetRecordDetail(Guid recordId)
        {
            var userClaim = User.Claims.FirstOrDefault(p => p.Type == "UserId");
            Guid? userId = null;
            if (userClaim != null)
            {
                userId = Guid.Parse(userClaim.Value);
            }
            var response = await _service.GetRecordDetail(recordId);
            return Ok(new BaseResponse(StatusCodes.Status200OK, "Get record detail successfully", response));
        }

    }
}
