using System.Text.Json.Serialization;

namespace PoemTown.Repository.CustomException;

public class CoreException : Exception
{
    public CoreException(int statusCode, string? errorMessage)
        : base(errorMessage)
    {
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }

    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; }
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }
}