using System.ComponentModel.DataAnnotations;

namespace PoemTown.Service.BusinessModels.RequestModels.AccountRequests;

public class ResendEmailConfirmationRequest
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public string Email { get; set; }
}