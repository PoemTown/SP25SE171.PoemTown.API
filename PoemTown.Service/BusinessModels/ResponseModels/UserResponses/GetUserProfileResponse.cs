using PoemTown.Repository.Enums.Accounts;

namespace PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

public class GetUserProfileResponse
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string UserName { get; set; }
    public string Bio { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PhoneNumber { get; set; }
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
    public int TotalFollower { get; set; }
    public AccountStatus Status { get; set; }
}