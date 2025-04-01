using PoemTown.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Service.BusinessModels.ResponseModels.TargetMarkResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses
{
    public class GetCollectionResponse
    {
        public Guid Id { get; set; }
        public string CollectionName { get; set; } = default!;
        public string? CollectionDescription { get; set; } = default!;
        public string? CollectionImage { get; set; } = default!;
        public bool? IsDefault { get; set; } = false;
        public int? TotalChapter { get; set; } = default!;
        public int? TotalRecord { get; set; } = default!;
        public byte[]? RowVersion { get; set; }
        public bool? IsCommunity { get; set; }
        public bool? IsMine { get; set; } = false;
        public DateTimeOffset? CreatedTime { get; set; }
        public DateTimeOffset? LastUpdatedTime { get; set; }
        public GetTargetMarkResponse? TargetMark { get; set; }
        public GetBasicUserInformationResponse? User { get; set; }

    }
}
