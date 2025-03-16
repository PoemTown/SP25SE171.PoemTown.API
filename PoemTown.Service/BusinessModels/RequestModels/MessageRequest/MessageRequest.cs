using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.RequestModels.MessageRequest
{
    public class MessageRequest
    {
        public Guid ToUserId { get; set; }
        public string Message { get; set; }
    }
}
