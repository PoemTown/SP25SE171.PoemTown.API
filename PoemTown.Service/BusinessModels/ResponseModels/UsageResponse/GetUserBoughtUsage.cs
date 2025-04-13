using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.ResponseModels.UsageResponse
{
    public class GetUserBoughtUsage
    {
        public DateTime? CopyRightValidFrom { get; set; }

        public DateTime? CopyRightValidTo { get; set; }
        public GetBasicUserInformationResponse Buyer { get; set; }
    }
}
