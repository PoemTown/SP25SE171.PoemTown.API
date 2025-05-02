using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.QueryOptions.FilterOptions.BankTypeFilters;

public class GetUserBankTypeFilterOption
{
    [FromQuery(Name = "bankName")]
    public string? BankName { get; set; }
    [FromQuery(Name = "bankCode")]
    public string? BankCode { get; set; }
}