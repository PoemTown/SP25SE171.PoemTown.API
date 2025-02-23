using Microsoft.AspNetCore.Http;

namespace PoemTown.Service.ThirdParties.Models.AwsS3;

public class UploadImageToAwsS3Model
{
    public IFormFile File { get; set; }
    public int? Height { get; set; }
    public int? Width { get; set; }
    public int? Quality { get; set; }
    public string FolderName { get; set; }
}