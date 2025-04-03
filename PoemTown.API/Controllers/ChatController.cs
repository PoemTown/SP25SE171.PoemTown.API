using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Crmf;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.MessageRequest;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.ChatResponse;
using PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.CollectionFilters;
using PoemTown.Service.QueryOptions.FilterOptions.MessageFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.CollectionSorts;
using PoemTown.Service.QueryOptions.SortOptions.MessageSorts;

namespace PoemTown.API.Controllers
{
    public class ChatController : BaseController
    {
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;

        public ChatController(IChatService chatService, IMapper mapper)
        {
            _chatService = chatService;
            _mapper = mapper;

        }

        [Authorize]
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] MessageRequest request)
        {
            Guid fromUserId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            await _chatService.SendPrivateMessageAsync(fromUserId, request.ToUserId, request.Message);
            return Ok(new { status = "sent" });
        }


        /// <summary>
        /// Lấy danh sách tất cả user mình đã từng nhắn, yêu cầu đăng nhập
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/partner")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<GetBasicUserInformationResponse>>>
        GetChatPartners(RequestOptionsBase<GetChatPartnerFilter, GetChatPartnerSort> request)
        {
            
            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            
            var result = await _chatService.GetChatPartners(userId, request);

            var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetBasicUserInformationResponse>>(result);
            basePaginationResponse.StatusCode = StatusCodes.Status200OK;
            basePaginationResponse.Message = "Get Chat Partners successfully";
            return Ok(basePaginationResponse);
        }

        /// <summary>
        /// Lấy danh sách tất cả nội dung tin nhắn với người mình đã từng nhắn, yêu cầu đăng nhập
        /// </summary>
        /// <param name="request"></param>
        /// <param name="targetUserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/partner/content")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<GetMesssageWithPartner>>>
        GetPrivateMessagesWithUser(RequestOptionsBase<object, object> request, [FromQuery] Guid targetUserId)
        {

            Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);

            var result = await _chatService.GetPrivateMessagesWithUser(userId, targetUserId, request);

            var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetMesssageWithPartner>>(result);
            basePaginationResponse.StatusCode = StatusCodes.Status200OK;
            basePaginationResponse.Message = "Get Chat Partners successfully";
            return Ok(basePaginationResponse);
        }
    }
}
