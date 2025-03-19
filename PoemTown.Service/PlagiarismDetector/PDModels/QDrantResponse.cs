using System.Text.Json.Serialization;
using PoemTown.Service.CustomAttribute;

namespace PoemTown.Service.PlagiarismDetector.PDModels;

public class QDrantResponse<T>
{
    [JsonPropertyName("usage")]
    public Usage? Usage { get; set; }
    
    [JsonPropertyName("time")]
    public double Time { get; set; }
    
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    
    [JsonPropertyName("result")]
    public IList<T> Results { get; set; }
}

public class Usage
{
    [JsonPropertyName("cpu")]
    public int Cpu { get; set; }
    
    [JsonPropertyName("io_read")]
    public int IORead { get; set; }
    
    [JsonPropertyName("io_write")]
    public int IOWrite { get; set; }
}

public class QdrantErrorResponse
{
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
}