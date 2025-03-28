using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses
{
    public class GetBoughtRecordResponse : BaseEntity
    {
        public string? FileName { get; set; }
        public decimal Price { get; set; }
        public string FileUrl { get; set; }

        public GetPoemDetailResponse OriginalPoem {  get; set; }    
        public GetBasicUserInformationResponse Buyer { get; set; }
        public GetBasicUserInformationResponse Owner { get; set; }

    }
}
