using System.Text.Json.Serialization;

namespace PoemTown.Service.ThirdParties.Models.TheHiveAi;

public class TheHiveAiResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("model")]
    public string Model { get; set; }
    
    [JsonPropertyName("version")]
    public string Version { get; set; }
    
    [JsonPropertyName("input")]
    public Input Input { get; set; }
    
    [JsonPropertyName("output")]
    public List<Output> Output { get; set; }
}

public class Input
{
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }
    
    [JsonPropertyName("negative_prompt")]
    public string NegativePrompt { get; set; }

    [JsonPropertyName("seed")]
    public int Seed { get; set; }
}

public class Output
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public class HiveAiErrorResponse
{
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
}