using PoemTown.Repository.Base;
using PoemTown.Repository.Enums.SaleVersions;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.ResponseModels.UsageResponse
{
    public class GetPoemVersionResponse : BaseEntity
    {
        public Guid Id { get; set; }
        public int CommissionPercentage { get; set; }
        public decimal Price { get; set; }
        public int DurationTime { get; set; }
        public SaleVersionStatus Status { get; set; }
        public bool IsInUse { get; set; }
        public  List<GetUserBoughtUsage> UsageRights { get; set; }

    }
}
