using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.QueryOptions.FilterOptions.DepositCommissionSettingFilters;

public class GetDepositCommissionSettingFilterOption
{
    [FromQuery(Name = "isInUse")] 
    public bool? IsInUse { get; set; }
    
    [FromQuery(Name = "amountPercentage")]
    [Range(0, 100, ErrorMessage = "AmountPercentage must be between 0 and 100.")]
    public int? AmountPercentage { get; set; }
}