using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.ResponseModels.ChatResponse
{
    public class GetMesssageWithPartner
    {
        public Guid Id { get; set; }
        public string MessageText { get; set; } = default!;
        public GetBasicUserInformationResponse FromUser { get; set; } 
        public GetBasicUserInformationResponse ToUser { get; set; } 
        public DateTimeOffset CreatedTime { get; set; }

    }
}
