using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using PoemTown.Repository.Utils;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Models.AwsS3;
using PoemTown.Service.ThirdParties.Settings.AwsS3;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace PoemTown.Service.ThirdParties.Services;

public class AwsS3Service : IAwsS3Service
{
    private readonly AwsS3Settings _awsS3Settings;
    private readonly IAmazonS3 _amazonS3;

    public AwsS3Service(AwsS3Settings awsS3Settings, IAmazonS3 amazonS3)
    {
        _awsS3Settings = awsS3Settings;
        _amazonS3 = amazonS3;
    }

    public async Task UploadFileAsync(Stream filePath, string fileName)
    {
        var fileTransferUtility = new TransferUtility(_amazonS3);
        var fileTransferRequest = new TransferUtilityUploadRequest
        {
            InputStream = filePath,
            BucketName = _awsS3Settings.BucketName,
            Key = fileName,
        };

        // Optionally set the ContentType based on file extension or other logic
        await fileTransferUtility.UploadAsync(fileTransferRequest);
    }

    public async Task<GetObjectResponse> GetFileAsync(string fileName)
    {
        var getObjectRequest = new GetObjectRequest()
        {
            BucketName = _awsS3Settings.BucketName,
            Key = fileName
        };
        return await _amazonS3.GetObjectAsync(getObjectRequest);
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var deleteObjectRequest = new DeleteObjectRequest()
        {
            BucketName = _awsS3Settings.BucketName,
            Key = fileName
        };
        await _amazonS3.DeleteObjectAsync(deleteObjectRequest);
    }

    public async Task<string> UploadImageToAwsS3Async(UploadImageToAwsS3Model s3Model)
    {
        //Save file to temp folder
        var filePath = Path.GetTempFileName();
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await s3Model.File.CopyToAsync(stream);
        }

        //Load image
        using var image = await Image.LoadAsync(filePath);

        //Create output stream
        using var output = new MemoryStream();

        //Resize image if width and height are provided
        if (s3Model.Width != null && s3Model.Height != null)
        {
            image.Mutate(p => p.Resize(s3Model.Width.Value, s3Model.Height.Value));
        }

        //Set quality if provided
        var jpegEncoder = new JpegEncoder()
        {
            Quality = s3Model.Quality ?? 100,
        };
        await image.SaveAsJpegAsync(output, jpegEncoder);
        output.Seek(0, SeekOrigin.Begin);

        //Generate new file name
        string fileName = s3Model.File.FileName;
        var fileExtension = Path.GetExtension(fileName);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        string unixTimeStamp = TimeStampHelper.GenerateUnixTimeStampNow().ToString();

        fileName = $"{s3Model.FolderName}/"
                   + fileNameWithoutExtension + "-" + unixTimeStamp + fileExtension;

        await UploadFileAsync(output, fileName);
        return $"{_awsS3Settings.ServiceUrl}/{_awsS3Settings.BucketName}/{fileName}";
    }
}