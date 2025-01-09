namespace PoemTown.Service.BusinessModels.ResponseModels.AuthenResponses;

/// <summary>
/// Response model for login
/// </summary>
/// <param name="AccessToken">User's AccessToken</param>
/// <param name="RefreshToken">User's RefreshToken</param>
/// <param name="Role">User's Role</param>
public class LoginResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public IList<string> Role { get; set; }
}