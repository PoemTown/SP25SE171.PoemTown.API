using System.ComponentModel.DataAnnotations;

namespace PoemTown.Service.BusinessModels.RequestModels.AccountRequests;

public class ConfirmEmailRequest
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public string Email { get; set; }
    [Required]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "The value must be exactly 6 digits.")]
    public string EmailOtp { get; set; }
}