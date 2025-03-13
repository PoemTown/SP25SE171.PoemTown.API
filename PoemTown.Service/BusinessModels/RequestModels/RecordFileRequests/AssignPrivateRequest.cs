using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.RequestModels.RecordFileRequests
{
    public class AssignPrivateRequest
    {
        public Guid RecordId { get; set; }
        public decimal Price { get; set; }
    }
}
