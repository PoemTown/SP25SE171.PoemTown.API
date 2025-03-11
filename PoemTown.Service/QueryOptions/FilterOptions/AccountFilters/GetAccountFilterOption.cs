using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.Accounts;

namespace PoemTown.Service.QueryOptions.FilterOptions.AccountFilters;

public class GetAccountFilterOption
{
    [FromQuery(Name = "userName")]
    public string? UserName { get; set; }
    [FromQuery(Name = "email")]
    public string? Email { get; set; }
    [FromQuery(Name = "phoneNumber")]
    public string? PhoneNumner { get; set; }
    [FromQuery(Name = "status")]
    public AccountStatus? Status { get; set; }
    [FromQuery(Name = "roleId")]
    public Guid? RoleId { get; set; }
}