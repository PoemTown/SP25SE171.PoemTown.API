using PoemTown.Service.ThirdParties.Models.TheHiveAi;

namespace PoemTown.Service.ThirdParties.Interfaces;

public interface ITheHiveAiService
{
    Task<TheHiveAiResponse> ConvertTextToImageWithFluxSchnellEnhanced(ConvertTextToImageWithFluxSchnellEnhancedModel model);
    Task<TheHiveAiResponse> ConvertTextToImageWithSdxlEnhanced(ConvertTextToImageWithSdxlEnhancedModel model);
}