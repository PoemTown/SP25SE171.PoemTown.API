using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.PlagiarismDetector.PDModels;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;
using PoemTown.Service.ThirdParties.Models.TheHiveAi;

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
    /// CHÚ Ý REQUEST PARAMETER:
    ///
    /// - tất cả lấy từ request query
    /// 
    /// Status: Trạng thái của bài thơ
    ///
    /// - 0: Draft
    /// - 1: Posted
    /// - 2: Suspended
    ///
    /// Type:
    ///
    /// - ThoTuDo = 1,
    /// - ThoLucBat = 2,
    /// - ThoSongThatLucBat = 3,
    /// - ThoThatNgonTuTuyet = 4,
    /// - ThoNguNgonTuTuyet = 5,
    /// - ThoThatNgonBatCu = 6,
    /// - ThoBonChu = 7,
    /// - ThoNamChu = 8,
    /// - ThoSauChu = 9,
    /// - ThoBayChu = 10,
    /// - ThoTamChu = 11,
    ///
    /// SortOptions: Sắp xếp bài thơ theo thứ tự
    ///
    /// - 1: LikeCountAscending (Lượt thích tăng dần)
    /// - 2: LikeCountDescending (Lượt thích giảm dần)
    /// - 3: CommentCountAscending (Lượt bình luận tăng dần)
    /// - 4: CommentCountDescending (Lượt bình luận giảm dần)
    /// - 5: TypeAscending (Loại bài thơ theo chữ cái tăng dần a -> z)
    /// - 6: TypeDescending (Loại bài thơ theo chữ cái giảm dần z -> a)
    ///
    /// TargetMark Type:
    ///
    /// - Poem = 1,
    /// - Collection = 2
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/mine")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetPoemResponse>>> GetMyPoems(RequestOptionsBase<GetMyPoemFilterOption, GetMyPoemSortOption> request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        var paginationResponse = await _poemService.GetMyPoems(userId, request);

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
    /// <remarks>
    /// CHÚ Ý REQUEST PARAMETER:
    ///
    /// - poemId: lấy từ request path
    /// </remarks>
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
    /// <remarks>
    /// CHÚ Ý REQUEST PARAMETER:
    ///
    /// - poemId: lấy từ request path
    /// </remarks>
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

    /// <summary>
    /// Xóa một bài thơ trong bộ sưu tập cộng đồng, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// CHÚ Ý REQUEST PARAMETER:
    ///
    /// - poemId: lấy từ request path
    /// </remarks>
    /// <param name="poemId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("v1/{poemId}/community")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> DeletePoemInCommunity(Guid poemId)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _poemService.DeletePoemInCommunity(userId, poemId);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Poem deleted in community successfully"));
    }

    /// <summary>
    /// Lấy chi tiết của một bài thơ (Không bao gồm lịch sử chỉnh sửa), yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// Chỉ áp dụng filter, sort cũng như paging cho RecordFiles (Tức là poem không có filter, sort, paging)
    ///
    /// HIỆN TẠI FILTER CHƯA SỬ DỤNG CHO MỤC ĐÍCH NÀO NÊN KHÔNG CẦN ĐIỀN
    ///
    /// CHÚ Ý REQUEST PARAMETER:
    ///
    /// - poemId: lấy từ request path
    /// - tất cả còn lại lấy từ request query
    /// 
    /// Status: Trạng thái của bài thơ
    ///
    /// - 0: Draft
    /// - 1: Posted
    /// - 2: Suspended
    ///
    /// Type:
    ///
    /// - ThoTuDo = 1,
    /// - ThoLucBat = 2,
    /// - ThoSongThatLucBat = 3,
    /// - ThoThatNgonTuTuyet = 4,
    /// - ThoNguNgonTuTuyet = 5,
    /// - ThoThatNgonBatCu = 6,
    /// - ThoBonChu = 7,
    /// - ThoNamChu = 8,
    /// - ThoSauChu = 9,
    /// - ThoBayChu = 10,
    /// - ThoTamChu = 11,
    ///
    /// SortOptions: Sắp xếp bài thơ theo thứ tự:
    ///
    /// - CreatedTimeAscending = 1 (Thời gian tạo tăng dần),
    /// - CreatedTimeDescending = 2 (Thời gian tạo giảm dần) (Mặc định)
    /// 
    /// </remarks>
    /// <param name="poemId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/{poemId}/detail")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<GetPoemDetailResponse>>>
        GetPoemDetail(Guid poemId, RequestOptionsBase<GetPoemRecordFileDetailFilterOption, GetPoemRecordFileDetailSortOption> request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        var response = await _poemService.GetPoemDetail(userId, poemId, request);

        return Ok(new BaseResponse(StatusCodes.Status200OK, "Get poem detail successfully", response));
    }

    /// <summary>
    /// Lấy danh sách bài thơ đã được đăng tải, không yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// CHÚ Ý REQUEST PARAMETER:
    ///
    /// - tất cả lấy từ request query
    /// 
    /// Status: Trạng thái của bài thơ
    ///
    /// - 0: Draft
    /// - 1: Posted
    /// - 2: Suspended
    ///
    /// Type:
    ///
    /// - ThoTuDo = 1,
    /// - ThoLucBat = 2,
    /// - ThoSongThatLucBat = 3,
    /// - ThoThatNgonTuTuyet = 4,
    /// - ThoNguNgonTuTuyet = 5,
    /// - ThoThatNgonBatCu = 6,
    /// - ThoBonChu = 7,
    /// - ThoNamChu = 8,
    /// - ThoSauChu = 9,
    /// - ThoBayChu = 10,
    /// - ThoTamChu = 11,
    ///
    /// SortOptions: Sắp xếp bài thơ theo thứ tự
    ///
    /// - 1: LikeCountAscending (Lượt thích tăng dần)
    /// - 2: LikeCountDescending (Lượt thích giảm dần)
    /// - 3: CommentCountAscending (Lượt bình luận tăng dần)
    /// - 4: CommentCountDescending (Lượt bình luận giảm dần)
    /// - 5: TypeAscending (Loại bài thơ theo chữ cái tăng dần a -> z)
    /// - 6: TypeDescending (Loại bài thơ theo chữ cái giảm dần z -> a)
    ///
    /// TargetMark Type:
    ///
    /// - Poem = 1,
    /// - Collection = 2
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/posts")]
    public async Task<ActionResult<BaseResponse<GetPostedPoemResponse>>> GetPostedPoems(
        RequestOptionsBase<GetPoemsFilterOption, GetPoemsSortOption> request)
    {
        var userClaim = User.Claims.FirstOrDefault(p => p.Type == "UserId");
        Guid? userId = null;
        if (userClaim != null)
        {
            userId = Guid.Parse(userClaim.Value);
        }
        var paginationResponse = await _poemService.GetPostedPoems(userId, request);
        
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetPostedPoemResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get poems successfully";
        
        return Ok(basePaginationResponse);
    }
    
    
    /// <summary>
    /// Lấy danh sách bài thơ theo bộ sưu tập, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// TargetMark Type:
    ///
    /// - Poem = 1,
    /// - Collection = 2
    /// </remarks>
    /// <param name="request"></param>
    /// <param name="collectionId">Lấy từ request path</param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/{collectionId}")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetPoemInCollectionResponse>>> 
        GetPoemsInCollection(Guid collectionId, RequestOptionsBase<GetMyPoemFilterOption, GetMyPoemSortOption> request)
    {
        var userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        var paginationResponse = await _poemService.GetPoemsInCollection(userId, collectionId, request);

        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetPoemInCollectionResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get poems successfully";

        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Lấy danh sách bài thơ trending, không yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// CHÚ Ý REQUEST PARAMETER:
    ///
    /// - tất cả lấy từ request query
    ///
    /// Type:
    ///
    /// - ThoTuDo = 1,
    /// - ThoLucBat = 2,
    /// - ThoSongThatLucBat = 3,
    /// - ThoThatNgonTuTuyet = 4,
    /// - ThoNguNgonTuTuyet = 5,
    /// - ThoThatNgonBatCu = 6,
    /// - ThoBonChu = 7,
    /// - ThoNamChu = 8,
    /// - ThoSauChu = 9,
    /// - ThoBayChu = 10,
    /// - ThoTamChu = 11,
    ///
    /// SortOptions: Sắp xếp bài thơ theo thứ tự
    ///
    /// - 1: LikeCountAscending (Lượt thích tăng dần)
    /// - 2: LikeCountDescending (Lượt thích giảm dần)
    /// - 3: CommentCountAscending (Lượt bình luận tăng dần)
    /// - 4: CommentCountDescending (Lượt bình luận giảm dần)
    /// - 5: TypeAscending (Loại bài thơ theo chữ cái tăng dần a -> z)
    /// - 6: TypeDescending (Loại bài thơ theo chữ cái giảm dần z -> a)
    ///
    /// TargetMark Type:
    ///
    /// - Poem = 1,
    /// - Collection = 2
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/trending")]
    public async Task<ActionResult<BaseResponse<GetPostedPoemResponse>>> GetTrendingPoems(RequestOptionsBase<GetPoemsFilterOption, GetPoemsSortOption> request)
    {
        var userClaim = User.Claims.FirstOrDefault(p => p.Type == "UserId");
        Guid? userId = null;
        if (userClaim != null)
        {
            userId = Guid.Parse(userClaim.Value);
        }        
        var paginationResponse = await _poemService.GetTrendingPoems(userId, request);
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetPostedPoemResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get poems successfully";

        return Ok(basePaginationResponse);
    }
    
    /// <summary>
    /// Upload hình ảnh cho bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/image")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<string>>> UploadPoemImage(IFormFile file)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        var response = await _poemService.UploadPoemImage(userId, file);
        return Ok(new BaseResponse<string>(StatusCodes.Status201Created, "Upload image successfully", response));
    }
    
    /// <summary>
    /// Kích hoạt bán bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/enable-selling")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> EnableSellingPoem(EnableSellingPoemRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _poemService.EnableSellingPoem(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Enable selling poem successfully"));
    }
    
    /// <summary>
    /// Mua bản quyền của một bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <param name="poemId"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("v1/purchase")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> PurchasePoem([FromQuery]Guid poemId)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _poemService.PurchasePoemCopyRight(userId, poemId);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Purchase poem successfully"));
    }

    
    
    /// <summary>
    /// AI gợi ý hoàn thiện bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// Type:
    ///
    /// - ThoTuDo = 1,
    /// - ThoLucBat = 2,
    /// - ThoSongThatLucBat = 3,
    /// - ThoThatNgonTuTuyet = 4,
    /// - ThoNguNgonTuTuyet = 5,
    /// - ThoThatNgonBatCu = 6,
    /// - ThoBonChu = 7,
    /// - ThoNamChu = 8,
    /// - ThoSauChu = 9,
    /// - ThoBayChu = 10,
    /// - ThoTamChu = 11,
    ///
    /// maxToken: Số lượng token tối đa mà AI sẽ hoàn thiện (100 token xấp xỉ 750 chữ)
    /// </remarks>
    /// <summary>
    /// Mua bản quyền của một bài thơ, yêu cầu đăng nhập
    /// </summary>
    /// <param name="poemId"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/ai-chat-completion")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<string>>> PoemAiChatCompletion(PoemAiChatCompletionRequest request)
    {
        var response = await _poemService.PoemAiChatCompletion(request);
        return Ok(new BaseResponse<string>(StatusCodes.Status200OK, "Poem completion successfully", response));
    }

    /// <summary>
    /// Chuyển đổi văn bản thành hình ảnh (Xài ngon nhưng mà mắc) (Sử dụng OpenAI), yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// ImageSize:
    ///
    /// - Size1024X1024 (1024 x 1024) = 1,
    /// - Size1024X1792 (1024 x 1792) = 2,
    ///
    /// ImageStyle:
    ///
    /// - Natural = 1,
    /// - Vivid = 2,
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/text-to-image/open-ai")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<string>>> PoemTextToImage(ConvertPoemTextToImageRequest request)
    {
        var response = await _poemService.ConvertPoemTextToImage(request);
        return Ok(new BaseResponse<string>(StatusCodes.Status200OK, "Poem text to image successfully", response));
    } 

    /// <summary>
    /// Chuyển đổi văn bản thành hình ảnh (Hình ảnh trả ra khá khác so với prompt, hạn chế sử dụng) (Sử dụng The Hive AI với model Flux Schnell Enhanced), yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// - poemText: Văn bản cần chuyển đổi (Bản tiếng việt sẽ tự động dịch sang tiếng Anh)
    /// - prompt: Sử dụng để tạo hình ảnh (VIẾT BẰNG TIẾNG ANH) (Ví dụ: Render an image base on poem content)
    /// - negativePrompt: Sử dụng khi muốn loại bỏ từ khóa trong Prompt (VIẾT BẰNG TIẾNG ANH) (Ví dụ: Image response must not contain any text)
    /// - numberInferenceSteps: Số lượng bước suy luận (Mặc định là 4) (Càng cao thì ảnh càng chi tiết, tuy nhiên tốn nhiều tiền)
    /// - numberOfImages: Số lượng hình ảnh cần tạo (Mặc định là 1) (Khuyến khích để 1)
    /// - outPutFormat: Định dạng hình ảnh (Mặc định là Jpeg)
    /// - outPutQuality: Chất lượng hình ảnh (Mặc định là 100%)
    ///  
    /// ImageSize:
    ///
    /// - Width1344Height768 (Width: 1344, Height: 768) = 1,
    /// - Width1280Height960 (Width: 1280, Height: 960) = 2,
    /// - Width960Height1280 (Width: 960, Height: 1280) = 3,
    /// - Width768Height1344 (Width: 768, Height: 1344) = 4,
    /// - Width1024Height1024 (Width: 1024, Height: 1024) = 5,
    ///
    /// OutPutFormat:
    ///
    /// - Jpeg = 1,
    /// - Png = 2,
    /// </remarks>
    /// <param name="fluxSchnellEnhancedRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/text-to-image/the-hive-ai/flux-schnell-enhanced")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<TheHiveAiResponse>>>
        PoemTextToImageWithTheHiveAiFluxSchnellEnhanced(ConvertPoemTextToImageWithTheHiveAiFluxSchnellEnhancedRequest fluxSchnellEnhancedRequest)
    {
        var response = await _poemService.ConvertPoemTextToImageWithTheHiveAiFluxSchnellEnhanced(fluxSchnellEnhancedRequest);
        return Ok(new BaseResponse<TheHiveAiResponse>(StatusCodes.Status200OK, "Poem text to image with The Hive AI successfully", response));
    }

    /// <summary>
    /// Chuyển đổi văn bản thành hình ảnh (NÊN XÀI CÁI NÀY) (Sử dụng The Hive AI với model SDXL Enhanced), yêu cầu đăng nhập
    /// </summary>
    /// <remarks>
    /// - poemText: Văn bản cần chuyển đổi (Bản tiếng việt sẽ tự động dịch sang tiếng Anh)
    /// - prompt: Sử dụng để tạo hình ảnh (VIẾT BẰNG TIẾNG ANH) (Ví dụ: Render an image base on poem content)
    /// - negativePrompt: Sử dụng khi muốn loại bỏ từ khóa trong Prompt (VIẾT BẰNG TIẾNG ANH) (Ví dụ: Image response must not contain any text)
    /// - numberInferenceSteps: Số lượng bước suy luận (Mặc định là 4) (Càng cao thì ảnh càng chi tiết, tuy nhiên tốn nhiều TÀI NGUYÊN)
    /// - guidanceScale: Tỷ lệ chính xác mà hình trả ra dựa trên prompt (càng cao thì càng giống với prompt mô tả, tuy nhiên tốn nhiều TÀI NGUYÊN)
    /// - numberOfImages: Số lượng hình ảnh cần tạo (Mặc định là 1) (Khuyến khích để 1)
    /// - outPutFormat: Định dạng hình ảnh (Mặc định là Jpeg)
    /// - outPutQuality: Chất lượng hình ảnh (Mặc định là 100%)
    ///  
    /// ImageSize:
    ///
    /// - Width1344Height768 (Width: 1344, Height: 768) = 1,
    /// - Width1280Height960 (Width: 1280, Height: 960) = 2,
    /// - Width960Height1280 (Width: 960, Height: 1280) = 3,
    /// - Width768Height1344 (Width: 768, Height: 1344) = 4,
    /// - Width1024Height1024 (Width: 1024, Height: 1024) = 5,
    ///
    /// OutPutFormat:
    ///
    /// - Jpeg = 1,
    /// - Png = 2,
    /// </remarks>
    /// <param name="fluxSchnellEnhancedRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/text-to-image/the-hive-ai/sdxl-enhanced")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<TheHiveAiResponse>>>
        PoemTextToImageWithTheHiveAiSdxlEnhanced(
            ConvertPoemTextToImageWithTheHiveAiSdxlEnhancedRequest fluxSchnellEnhancedRequest)
    {
        var response = await _poemService.ConvertPoemTextToImageWithTheHiveAiSdxlEnhanced(fluxSchnellEnhancedRequest);
        return Ok(new BaseResponse<TheHiveAiResponse>(StatusCodes.Status200OK,
            "Poem text to image with The Hive AI successfully", response));
    }

    [HttpPost]
    [Route("v1/community")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> CreatePoemInCommunity(CreateNewPoemRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _poemService.CreatePoemInCommunity(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status202Accepted, "Create poem in community successfully"));
    }
    
    /// <summary>
    /// Chuyển đổi thơ sang vector embedding, lưu vào QDrant database (Mục đích dùng để train trong việc phát hiện ĐẠO VĂN, có thể không cần lên giao diện), yêu cầu đăng nhập dưới quyền ADMIN
    /// </summary>
    /// <param name="poemId"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/store/{poemId}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<BaseResponse>> ConvertPoemIntoEmbeddingAndSaveToQdrant(Guid poemId)
    {
        await _poemService.ConvertPoemIntoEmbeddingAndSaveToQdrant(poemId);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Store poem embedding successfully"));
    }
    
    /// <summary>
    /// Kiểm tra bài thơ có bị đạo văn không, yêu cầu đăng nhập
    /// </summary>
    /// <param name="poemContent"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("v1/plagiarism")]
    [Authorize]
    public async Task<ActionResult<BaseResponse<PoemPlagiarismResponse>>>
        SearchSimilarPoemEmbeddingPoint([FromQuery]string poemContent)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        var response = await _poemService.CheckPoemPlagiarism(userId, poemContent);
        return Ok(new BaseResponse<PoemPlagiarismResponse>(StatusCodes.Status200OK, "Check poem plagiarism successfully", response));
    }
}