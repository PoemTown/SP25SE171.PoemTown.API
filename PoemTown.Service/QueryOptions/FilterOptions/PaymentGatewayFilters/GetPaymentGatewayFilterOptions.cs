using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.QueryOptions.FilterOptions.PaymentGatewayFilters;

public class GetPaymentGatewayFilterOptions
{
    [FromQuery(Name = "isSuspended")]
    public bool? IsSuspended { get; set; } = false;
}