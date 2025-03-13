using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using PoemTown.Repository.Utils;
using PoemTown.Service.ThirdParties.Interfaces;
using PoemTown.Service.ThirdParties.Models.TheHiveAi;
using PoemTown.Service.ThirdParties.Settings.TheHiveAi;

namespace PoemTown.Service.ThirdParties.Services;

public class TheHiveAiService : ITheHiveAiService
{
    private readonly TheHiveAiSettings _theHiveAiSettings;
    private readonly IHttpClientFactory _httpClientFactory;
    private static string FluxSchnellEnhancedUrl = "https://api.thehive.ai/api/v3/hive/flux-schnell-enhanced";
    private static string SdxlEnhancedUrl = "https://api.thehive.ai/api/v3/hive/sdxl-enhanced";
    
    public TheHiveAiService(TheHiveAiSettings theHiveAiSettings,
        IHttpClientFactory httpClientFactory)
    {
        _theHiveAiSettings = theHiveAiSettings;
        _httpClientFactory = httpClientFactory;
    }

    public (string width, string height) GetImageSize(TheHiveAiImageSize imageSize)
    {
        return imageSize switch
        {
            TheHiveAiImageSize.Width768Height1344 => ("768", "1344"),
            TheHiveAiImageSize.Width960Height1280 => ("960", "1280"),
            TheHiveAiImageSize.Width1024Height1024 => ("1024", "1024"),
            TheHiveAiImageSize.Width1280Height960 => ("1280", "960"),
            TheHiveAiImageSize.Width1344Height768 => ("1344", "768"),
            _ => throw new ArgumentOutOfRangeException(nameof(imageSize), imageSize, null)
        };
    }
    
    public async Task<TheHiveAiResponse> ConvertTextToImageWithFluxSchnellEnhanced(ConvertTextToImageWithFluxSchnellEnhancedModel model)
    {
        var client = _httpClientFactory.CreateClient();
        // Get image size base on enum
        var imageSize = GetImageSize(model.ImageSize);
        
        // Create request body
        var requestBody = new
        {
            input = new {
                prompt = $"Prompt: {model.Prompt}\nNegative Prompt: {model.NegativePrompt}",
                image_size = new
                {
                    width = imageSize.width,
                    height = imageSize.height  
                },
                num_inference_steps = model.NumberInferenceSteps,
                num_images = model.NumberOfImages,
                seed = Math.Abs((int)TimeStampHelper.GenerateUnixTimeStamp(0, 0, 0, true)),
                output_format = EnumHelper.GetDescription(model.OutPutFormat),
                output_quality = model.OutPutQuality
            }
        };
        
        // Serialize request body
        JsonSerializer.Serialize(requestBody);

        // Add authorization header
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_theHiveAiSettings.ApiKey}");
        var response = await client.PostAsJsonAsync(FluxSchnellEnhancedUrl, requestBody);

        // Read response content
        var responseContent = await response.Content.ReadAsStringAsync();
        
        // Check if response is not successful
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = JsonSerializer.Deserialize<HiveAiErrorResponse>(responseContent);
            throw new Exception($"Hive AI Error: {errorResponse?.Message ?? "Unknown error"}");
        }
        
        var responseObject = JsonSerializer.Deserialize<TheHiveAiResponse>(responseContent);
        
        return responseObject;
    }
    
    public async Task<TheHiveAiResponse> ConvertTextToImageWithSdxlEnhanced(ConvertTextToImageWithSdxlEnhancedModel model)
    {
        var client = _httpClientFactory.CreateClient();
        
        // Get image size base on enum
        var imageSize = GetImageSize(model.ImageSize);
        
        // Create request body
        var requestBody = new
        {
            input = new {
                prompt = $"Prompt: {model.Prompt}",
                negative_prompt = $"Negative Prompt: {model.NegativePrompt}",
                image_size = new
                {
                    width = imageSize.width,
                    height = imageSize.height  
                },
                num_inference_steps = model.NumberInferenceSteps,
                num_images = model.NumberOfImages,
                guidance_scale = model.GuidanceScale,
                seed = Math.Abs((int)TimeStampHelper.GenerateUnixTimeStamp(0, 0, 0, true)),
                output_format = EnumHelper.GetDescription(model.OutPutFormat),
                output_quality = model.OutPutQuality
            }
        };
        
        // Serialize request body
        JsonSerializer.Serialize(requestBody);
        
        // Add authorization header
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_theHiveAiSettings.ApiKey}");
        var response = await client.PostAsJsonAsync(SdxlEnhancedUrl, requestBody);
        
        // Read response content
        var responseContent = await response.Content.ReadAsStringAsync();

        // Check if response is not successful
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = JsonSerializer.Deserialize<HiveAiErrorResponse>(responseContent);
            throw new Exception($"Hive AI Error: {errorResponse?.Message ?? "Unknown error"}");
        }

        var responseObject = JsonSerializer.Deserialize<TheHiveAiResponse>(responseContent);
        
        return responseObject;
    }
}