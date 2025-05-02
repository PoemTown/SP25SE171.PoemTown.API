using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.BankTypes;

namespace PoemTown.Service.QueryOptions.FilterOptions.BankTypeFilters;

public class GetBankTypeFilterOption
{
    [FromQuery(Name = "bankName")]
    public string? BankName { get; set; }
    [FromQuery(Name = "bankCode")]
    public string? BankCode { get; set; }
    [FromQuery(Name = "status")]
    public BankTypeStatus? Status {get; set; }
}