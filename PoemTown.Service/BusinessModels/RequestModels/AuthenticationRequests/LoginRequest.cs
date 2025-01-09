namespace PoemTown.Service.BusinessModels.RequestModels.AuthenticationRequests;

/// <summary>
/// Login request model
/// </summary>
/// <param name="Email">User's Email</param>
/// <param name="Password">User's Password</param>
public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}