using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace PoemTown.Service.QueryOptions.ExtensionOptions;

public class PriceRange : IValidatableObject
{
    [FromQuery(Name = "priceFrom")]
    public decimal? PriceFrom { get; set; } = 0;
    // Default value for PriceTo is 10,000,000
    [FromQuery(Name = "priceTo")]
    public decimal? PriceTo { get; set; } = 10000000;
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PriceFrom > PriceTo)
        {
            yield return new ValidationResult("'PriceFrom' cannot be greater than 'PriceTo'.");
        }
    }
}