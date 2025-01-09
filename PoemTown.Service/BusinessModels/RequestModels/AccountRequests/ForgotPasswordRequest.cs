using System.ComponentModel.DataAnnotations;

namespace PoemTown.Service.BusinessModels.RequestModels.AccountRequests;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}