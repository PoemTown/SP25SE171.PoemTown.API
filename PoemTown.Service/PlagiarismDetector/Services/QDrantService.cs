using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using PoemTown.Repository.CustomException;
using PoemTown.Service.PlagiarismDetector.Interfaces;
using PoemTown.Service.PlagiarismDetector.PDModels;
using PoemTown.Service.PlagiarismDetector.Settings;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using IEmbeddingService = PoemTown.Service.PlagiarismDetector.Interfaces.IEmbeddingService;

namespace PoemTown.Service.PlagiarismDetector.Services;

public class QDrantService : IQDrantService
{
    private QdrantClient _qdrantClient;
    private readonly IEmbeddingService _embeddingService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly QDrantSettings _qDrantSettings;
    private readonly IWebHostEnvironment _webHostEnvironment;
    
    private const string CollectionName = "poems";
    
    public QDrantService(QdrantClient qdrantClient, 
        IEmbeddingService embeddingService,
        IHttpClientFactory httpClientFactory,
        QDrantSettings qDrantSettings,
        IWebHostEnvironment webHostEnvironment)
    {
        _qdrantClient = qdrantClient;
        _embeddingService = embeddingService;
        _httpClientFactory = httpClientFactory;
        _qDrantSettings = qDrantSettings;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task StorePoemEmbeddingAsync(Guid poemId, Guid poetId, string poemText, bool? isFamousPoem = false)
    {
        var client = _httpClientFactory.CreateClient();
 
        // Generate embedding from OpenAI
        double[] embedding = await _embeddingService.GenerateEmbeddingFromOpenAI(poemText);
        
        // Create request body
        var requestBody = new
        {
            collection_name = CollectionName,
            points = new[]
            {
                new
                {
                    id = poemId.ToString(),
                    vector = embedding,
                    payload = new
                    {
                        poetId = poetId.ToString(),
                        poemContent = poemText,
                        isFamousPoem = isFamousPoem,
                    },
                }
            }
        };

        // Add API key to request header
        client.DefaultRequestHeaders.Add("api-key", $"{_qDrantSettings.ApiKey}");
        
        // Send request to QDrant
        var response = await client.PutAsJsonAsync($"{GetQDrantUri}/collections/{CollectionName}/points", requestBody);
        
        // Check if response is not successful and throw exception
        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<QdrantErrorResponse>(responseContent);
            throw new Exception($"QDrant error: {responseObject!.Message}");
        }
    }
    
    public async Task<QDrantResponse<SearchPointsResult>?> SearchSimilarPoemEmbeddingPoint(Guid userId, string poemText)
    {
        var client = _httpClientFactory.CreateClient();
        
        // Generate embedding from OpenAI
        double[] embedding = await _embeddingService.GenerateEmbeddingFromOpenAI(poemText);
        
        // Create request body
        var requestBody = new
        {
            collection_name = CollectionName,
            vector = embedding,
            limit = 10,
            filter = new
            {
                must_not = new[]
                {
                    new
                    {
                        key = "poetId",
                        match = new
                        {
                            value = userId.ToString()
                        }
                    }
                }
            }
        };
        
        // Add API key to request header
        client.DefaultRequestHeaders.Add("api-key", $"{_qDrantSettings.ApiKey}");
        
        // Send request to QDrant
        var response = await client.PostAsJsonAsync($"{GetQDrantUri}/collections/{CollectionName}/points/search", requestBody);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        
        // Check if response is not successful and throw exception
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = JsonSerializer.Deserialize<QdrantErrorResponse>(responseContent);
            throw new Exception($"QDrant error: {errorResponse!.Message}");
        }
        
        var responseObject = JsonSerializer.Deserialize<QDrantResponse<SearchPointsResult>>(responseContent);

        return responseObject;
    }

    public async Task DeletePoemEmbeddingPoint(IList<Guid> poemIds)
    {
        var client = _httpClientFactory.CreateClient();

        // Create request body
        var requestBody = new
        {
            collection_name = CollectionName,
            points = poemIds.Select(id => id.ToString()).ToArray(),
        };
        
        // Add API key to request header
        client.DefaultRequestHeaders.Add("api-key", $"{_qDrantSettings.ApiKey}");
        
        // Send request to QDrant
        var response = await client.PostAsJsonAsync($"{GetQDrantUri}/collections/{CollectionName}/points/delete", requestBody);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        
        // Check if response is not successful and throw exception
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = JsonSerializer.Deserialize<QdrantErrorResponse>(responseContent);
            throw new Exception($"QDrant error: {errorResponse!.Message}");
        }
    }
    
    private string GetQDrantUri => _webHostEnvironment.IsDevelopment()
            ? $"http://{_qDrantSettings.Host}:{_qDrantSettings.Port}"
            : $"https://{_qDrantSettings.Host}:{_qDrantSettings.Port}";
}