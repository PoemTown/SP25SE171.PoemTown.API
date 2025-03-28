using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
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
    private readonly IHttpClientFactory _httpClientFactory;
    public AwsS3Service(AwsS3Settings awsS3Settings, 
        IAmazonS3 amazonS3,
        IHttpClientFactory httpClientFactory)
    {
        _awsS3Settings = awsS3Settings;
        _amazonS3 = amazonS3;
        _httpClientFactory = httpClientFactory;
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

    public async Task<string> UploadAudioToAwsS3Async(UploadFileToAwsS3Model s3Model)
    {
        // Save file to temp folder
        var filePath = Path.GetTempFileName();
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await s3Model.File.CopyToAsync(stream);
        }

        // Generate new file name with timestamp
        string fileName = s3Model.File.FileName;
        var fileExtension = Path.GetExtension(fileName);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        string unixTimeStamp = TimeStampHelper.GenerateUnixTimeStampNow().ToString();

        fileName = $"{s3Model.FolderName}/"
                   + fileNameWithoutExtension + "-" + unixTimeStamp + fileExtension;

        // Upload file to AWS S3
        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        await UploadFileAsync(fileStream, fileName);

        // Return file URL
        return $"{_awsS3Settings.ServiceUrl}/{_awsS3Settings.BucketName}/{fileName}";
    }

    public async Task<string> DownloadAndUploadToS3Async(string imageUrl, string folderName)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            
            // Step 1: Download the image from OpenAI URL
            HttpResponseMessage response = await httpClient.GetAsync(imageUrl);
            response.EnsureSuccessStatusCode();
            byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

            // Step 2: Convert image bytes to an IFormFile
            IFormFile imageFile = ImageHelper.ConvertToIFormFile(imageBytes, $"ai-generated-image-{StringHelper.GenerateRandomString(10)}.png");
            
            ImageHelper.ValidateImage(imageFile);

            // Step 3: Upload image using your existing function
            var s3Model = new UploadImageToAwsS3Model
            {
                File = imageFile,
                FolderName = folderName, // Specify the folder in S3
                Width = null,  // Resize if needed
                Height = null, 
                Quality = 100  // Set quality
            };

            return await UploadImageToAwsS3Async(s3Model);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
    
    
}