namespace PoemTown.Service.ThirdParties.Models.TheHiveAi;

public class ConvertTextToImageWithSdxlEnhancedModel
{
    public string Prompt { get; set; }
    /// <summary>
    /// Optional. The negative prompt uses when want to exclude from prompt.
    /// </summary>
    public string? NegativePrompt { get; set; }
    public TheHiveAiImageSize ImageSize { get; set; } = TheHiveAiImageSize.Width1024Height1024;
    public int? NumberInferenceSteps { get; set; } = 4;
    public float? GuidanceScale { get; set; } = 5;
    public int? NumberOfImages { get; set; } = 1;
    // public int Seed { get; set; }
    public OutPutFormat OutPutFormat { get; set; } = OutPutFormat.Jpeg;
    public int? OutPutQuality { get; set; } = 100;
}
