namespace PoemTown.Service.BusinessModels.RequestModels.UserRequests;

public class UpdateMyProfileRequest
{
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Avatar { get; set; }
    public string? DisplayName { get; set; }
    public string? Bio { get; set; }
}