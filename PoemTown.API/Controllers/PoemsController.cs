using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;

namespace PoemTown.API.Controllers;

public class PoemsController : BaseController
{
    private readonly IPoemService _poemService;
    private readonly IMapper _mapper;
    public PoemsController(IPoemService poemService, IMapper mapper)
    {
        _poemService = poemService;
        _mapper = mapper;
    }

    /// <summary>
    /// Tạo mới một bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// Tất cả thuộc tính đều có thể NULL
    ///
    /// Status: Trạng thái của bài thơ, mặc định là Draft
    /// 
    /// - 0: Draft (Nháp)
    /// - 1: Posted (Đã đăng)
    /// - 2: Suspended (Không sử dụng)
    ///
    /// Type: Loại bài thơ, thể thơ:
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> CreateNewPoem(CreateNewPoemRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _poemService.CreateNewPoem(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Poem created successfully"));
    }
    
    /// <summary>
    /// Lấy danh sách bài thơ của tôi, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// Status: Trạng thái của bài thơ
    ///
    /// - 0: Draft
    /// - 1: Posted
    /// - 2: Suspended
    ///
    /// Type: Loại bài thơ, thể thơ:
    ///
    /// SortOptions: Sắp xếp bài thơ theo thứ tự
    ///
    /// - 0: LikeCountAscending (Lượt thích tăng dần)
    /// - 1: LikeCountDescending (Lượt thích giảm dần)
    /// - 2: CommentCountAscending (Lượt bình luận tăng dần)
    /// - 3: CommentCountDescending (Lượt bình luận giảm dần)
    /// - 4: TypeAscending (Loại bài thơ theo chữ cái tăng dần a -> z)
    /// - 5: TypeDescending (Loại bài thơ theo chữ cái giảm dần z -> a)
    /// </remarks>
    /// <param name="reqeust"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/mine")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetPoemResponse>>> GetMyPoems(RequestOptionsBase<GetMyPoemFilterOption, GetMyPoemSortOption> reqeust)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        var paginationResponse = await _poemService.GetMyPoems(userId, reqeust);
        
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetPoemResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get poems successfully";
        
        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Chỉnh sửa một bài thơ, sau đó tạo ra bản sao (history) của bài thơ trước khi chỉnh sửa, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// Tất cả thuộc tính đều có thể NULL
    ///
    /// MỘT BÀI THƠ CHỈ CÓ THỂ CHỈNH SỬA KHI STATUS = 0 (DRAFT)
    /// 
    /// Status: Trạng thái của bài thơ
    /// 
    /// - 0: Draft (Nháp)
    /// - 1: Posted (Đã đăng)
    /// - 2: Suspended (Không sử dụng)
    ///
    /// Type: Loại bài thơ, thể thơ:
    /// </remarks>

    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> UpdatePoem(UpdatePoemRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _poemService.UpdatePoem(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Poem updated successfully"));
    }
    
    /// <summary>
    /// Xóa một bài thơ (Chuyển vào thùng rác), yêu cầu đăng nhập
    /// </summary>
    /// <param name="poemId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{poemId}")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeletePoem(Guid poemId)
    {
        await _poemService.DeletePoem(poemId);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Poem deleted successfully"));
    }
    
    /// <summary>
    /// Xóa một bài thơ (vĩnh viễn), yêu cầu đăng nhập
    /// </summary>
    /// <param name="poemId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{poemId}/permanent")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeletePoemPermanent(Guid poemId)
    {
        await _poemService.DeletePoemPermanent(poemId);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Poem deleted permanently successfully"));
    }
}