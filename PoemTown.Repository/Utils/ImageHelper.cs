using Microsoft.AspNetCore.Http;
using PoemTown.Repository.CustomException;

namespace PoemTown.Repository.Utils;

public static class ImageHelper
{
    private static readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

    public static bool ValidateImages(IList<IFormFile> files)
    {
        if (files == null || files.Count == 0)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "File is required");
        }

        foreach (var file in files)
        {
            // Check the content type
            if (!file.ContentType.StartsWith("image/"))
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Only image files are allowed");
            }

            // Check file extension
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "File extension is not allowed. Only .jpg, .jpeg, .png, .gif are allowed.");
            }

            // Optionally check file signature
            if (!IsImage(file))
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Content does not match the expected image type");
            }
        }

        // Return validation result
        return true;
    }

    public static bool ValidateImage(IFormFile file)
    {
        // Check the content type
        if (!file.ContentType.StartsWith("image/"))
        {
            throw new BadImageFormatException("Only image files are allowed.");
        }

        // Check file extension
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
        {
            throw new BadImageFormatException("File extension is not allowed. Only .jpg, .jpeg, .png, .gif are allowed.");
        }

        // Optionally check file signature
        if (!IsImage(file))
        {
            throw new BadImageFormatException("Content does not match the expected image type.");
        }

        // Return validation result
        return true;
    }
    private static bool IsImage(IFormFile file)
    {
        byte[] buffer = new byte[8];
        using (var stream = file.OpenReadStream())
        {
            stream.Read(buffer, 0, buffer.Length);
        }

        // JPG (FFD8FF), PNG (89504E47), GIF (47494638)
        var jpgSignature = new byte[] { 0xFF, 0xD8, 0xFF };
        var pngSignature = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        var gifSignature = new byte[] { 0x47, 0x49, 0x46, 0x38 };

        return buffer.Take(3).SequenceEqual(jpgSignature) ||
               buffer.Take(4).SequenceEqual(pngSignature) ||
               buffer.Take(4).SequenceEqual(gifSignature);
    }
    
    public static IFormFile ConvertToIFormFile(byte[] fileBytes, string fileName)
    {
        var stream = new MemoryStream(fileBytes);
        return new FormFile(stream, 0, fileBytes.Length, "image", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };
    }
}