using System.ComponentModel;
using PoemTown.Repository.Utils;

namespace PoemTown.Service.ThirdParties.Models.TheHiveAi;

public class ConvertTextToImageWithFluxSchnellEnhancedModel
{
    public string Prompt { get; set; }
    /// <summary>
    /// Optional. The negative prompt uses when want to exclude from prompt.
    /// </summary>
    public string? NegativePrompt { get; set; }
    public TheHiveAiImageSize ImageSize { get; set; } = TheHiveAiImageSize.Width1024Height1024;
    public int? NumberInferenceSteps { get; set; } = 4;
    public int? NumberOfImages { get; set; } = 1;
    // public int Seed { get; set; }
    public OutPutFormat OutPutFormat { get; set; } = OutPutFormat.Jpeg;
    public int? OutPutQuality { get; set; } = 100;
}

public enum TheHiveAiImageSize {
    [Description("Width: 1344, Height: 768")]
    Width1344Height768 = 1,
    [Description("width: 1280, Height: 960")]
    Width1280Height960 = 2,
    [Description("Width: {960}, Height: 1280")]
    Width960Height1280 = 3,
    [Description("Width: 768, Height: 1344")]
    Width768Height1344 = 4,
    [Description("Width: 1024, Height: 1024")]
    Width1024Height1024 = 5,
}

public enum OutPutFormat
{
    [Description("jpeg")]
    Jpeg = 1,
    [Description("png")]
    Png = 2,
}