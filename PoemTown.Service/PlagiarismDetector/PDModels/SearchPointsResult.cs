using System.Text.Json.Serialization;

namespace PoemTown.Service.PlagiarismDetector.PDModels;

public class SearchPointsResult
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("version")]
    public int Version { get; set; }
    
    [JsonPropertyName("score")]
    public double Score { get; set; }
}