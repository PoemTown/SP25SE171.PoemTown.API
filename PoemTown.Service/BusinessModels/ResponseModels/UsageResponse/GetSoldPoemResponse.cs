using PoemTown.Repository.Enums.UsageRights;
using PoemTown.Repository.Enums.UserPoems;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.SaleVersionResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.ResponseModels.UsageResponse
{
    public class GetSoldPoemResponse
    {
        public UsageRightStatus Status { get; set; }
        public DateTime? CopyRightValidFrom { get; set; }
        public DateTime? CopyRightValidTo { get; set; }
        public GetSaleVersionResponse SaleVersion { get; set; }

        public GetPoemDetailResponse Poem { get; set; }
        public GetBasicUserInformationResponse Buyer { get; set; }
        public GetBasicUserInformationResponse Owner { get; set; }
    }
}
