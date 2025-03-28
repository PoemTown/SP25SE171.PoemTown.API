using PoemTown.Service.ThirdParties.Models.AwsS3;

namespace PoemTown.Service.ThirdParties.Interfaces;

public interface IAwsS3Service
{
    Task UploadFileAsync(Stream filePath, string fileName);
    Task DeleteFileAsync(string fileName);
    Task<string> UploadImageToAwsS3Async(UploadImageToAwsS3Model s3Model);
    Task<string> UploadAudioToAwsS3Async(UploadFileToAwsS3Model s3Model);
    Task<string> DownloadAndUploadToS3Async(string imageUrl, string folderName);
}