using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.RecordFileResponses
{
    public class GetSoldRecordResponse
    {
        public Guid? Id { get; set; }
        public string? FileName { get; set; }
        public decimal Price { get; set; }
        public string FileUrl { get; set; }
        public int? TotalView { get; set; }

        public GetPoemDetailResponse Poem {  get; set; }

        public GetBasicUserInformationResponse Owner {  get; set; }
        public List<GetBasicUserInformationResponse> Buyers { get; set; }

    }
}
