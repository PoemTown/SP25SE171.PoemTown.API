using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.ThirdParties.Models.AwsS3
{
    public class UploadFileToAwsS3Model
    {
        public IFormFile File { get; set; } 
        public string FolderName { get; set; }
    }
}
