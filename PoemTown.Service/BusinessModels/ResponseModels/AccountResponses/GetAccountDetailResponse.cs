using PoemTown.Repository.Enums.Accounts;
using PoemTown.Service.BusinessModels.ResponseModels.RoleResponse;

namespace PoemTown.Service.BusinessModels.ResponseModels.AccountResponses;

public class GetAccountDetailResponse
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? DisplayName { get; set; }
    public string? Avatar { get; set; }
    public AccountStatus? Status { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string UserName { get; set; }    
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset LastUpdatedTime { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    public IEnumerable<GetRoleResponse> Roles { get; set; }
}