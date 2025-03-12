using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.Transactions;

namespace PoemTown.Service.QueryOptions.FilterOptions.TransactionFilters;

public class GetTransactionFilterOption
{
    [FromQuery(Name = "type")]
    public TransactionType? Type { get; set; }
    [FromQuery(Name = "email")]
    public string? Email { get; set; }
    [FromQuery(Name = "phoneNumber")]
    public string? PhoneNumber { get; set; }
}