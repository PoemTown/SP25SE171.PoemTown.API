using System.Security.Cryptography;
using System.Text;

namespace PoemTown.Repository.Utils;

public static class HmacSHA256Hasher
{
    public static string Hash(string input)
    {
        
        string? secret = ReadConfigurationHelper.GetEnvironmentVariable("SecretKeyHmacSHA256:SecretKey");
        if (string.IsNullOrWhiteSpace(secret))
        {
            var configuration = ReadConfigurationHelper.ReadDevelopmentAppSettings();
            secret = configuration["SecretKeyHmacSHA256:SecretKey"];
        }

        if (string.IsNullOrEmpty(secret))
        {
            throw new Exception("Secret cannot be null or empty");
        }

        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException("Input cannot be null or empty");
        }

        // Convert input to byte array
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);

        // Create an HMACSHA256 instance with the key
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        // Compute the hash
        byte[] hashBytes = hmac.ComputeHash(inputBytes);

        // Convert the hash to a hexadecimal string
        var sb = new StringBuilder();
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("x2")); // Convert each byte to hex
        }
        return sb.ToString();
    }
}