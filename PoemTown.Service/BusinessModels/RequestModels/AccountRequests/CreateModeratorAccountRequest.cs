using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PoemTown.Service.BusinessModels.RequestModels.AccountRequests;

public class CreateModeratorAccountRequest
{
    
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }

    [RegularExpression(@"^[\p{L}[\s]+$", ErrorMessage = "Full name should only contain letters and spaces, no special characters or numbers")]
    [DefaultValue("string")]
    public string? FullName { get; set; }
    
    [Phone(ErrorMessage = "Invalid phone number")]
    [DefaultValue("0909123456")]
    public string PhoneNumber { get; set; }
}