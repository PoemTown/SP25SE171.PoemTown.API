using PoemTown.Service.ThirdParties.Models.TheHiveAi;

namespace PoemTown.Service.BusinessModels.RequestModels.PoemRequests;

public class ConvertPoemTextToImageWithTheHiveAiSdxlEnhancedRequest
{
    public TheHiveAiImageSize ImageSize { get; set; }
    public string PoemText { get; set; }
    public string Prompt { get; set; }
    public string? NegativePrompt { get; set; } = "";
    public int? NumberInferenceSteps { get; set; } = 4;
    public float? GuidanceScale { get; set; } = 5;
    public int? NumberOfImages { get; set; } = 1;
    public OutPutFormat OutPutFormat { get; set; } = OutPutFormat.Jpeg;
    public int? OutPutQuality { get; set; } = 100;
}