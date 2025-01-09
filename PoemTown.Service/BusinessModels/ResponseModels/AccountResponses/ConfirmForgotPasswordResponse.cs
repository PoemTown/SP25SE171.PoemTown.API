namespace PoemTown.Service.BusinessModels.ResponseModels.AccountResponses;

public class ConfirmForgotPasswordResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}