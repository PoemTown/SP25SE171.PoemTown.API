using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Crmf;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.MessageRequest;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.Controllers
{
    public class ChatController : BaseController
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [Authorize]
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] MessageRequest request)
        {
            Guid fromUserId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
            await _chatService.SendPrivateMessageAsync(fromUserId, request.ToUserId, request.Message);
            return Ok(new { status = "sent" });
        }

    }
}
