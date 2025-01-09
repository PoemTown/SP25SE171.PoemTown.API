namespace PoemTown.Service.BusinessModels.ResponseModels.TokenResponses;

/// <summary>
/// Response model for token
/// </summary>
/// <param name="AccessToken"></param>
/// <param name="RefreshToken"></param>
public class TokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}