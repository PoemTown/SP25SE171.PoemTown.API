using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.ResponseModels.ChatResponse
{
    public class GetChatPartner
    {
        public Guid Id { get; set; }
        public string? DisplayName { get; set; }
        public string? UserName { get; set; }
        public string? Avatar { get; set; }
        public GetMesssageWithPartner LastMessage { get; set; }
    }
}
