using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.WithdrawalForm;

namespace PoemTown.Service.QueryOptions.FilterOptions.WithdrawalFormFilters;

public class GetWithdrawalFormFilterOption
{
    [FromQuery(Name = "status" )]
    public WithdrawalFormStatus? Status { get; set; }
    
    [FromQuery(Name = "userName")]
    public string? UserName { get; set; }
}