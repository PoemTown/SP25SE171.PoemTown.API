using System.ComponentModel.DataAnnotations;
using System.Reflection;
using PoemTown.Service.QueryOptions.RequestOptions;

namespace PoemTown.Service.CustomAttribute;

public class DynamicPageSizeAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is int pageSize)
        {
            // Retrieve the 'AllowExceedPageSize' property dynamically using reflection
            var allowExceedProperty = validationContext
                .ObjectType
                .GetProperty(nameof(RequestOptionsBase<object, object>.AllowExceedPageSize));
            
            if (allowExceedProperty == null)
            {
                return new ValidationResult("AllowExceedPageSize property not found.");
            }

            bool allowExceed = (bool)(allowExceedProperty.GetValue(validationContext.ObjectInstance) ?? false);
            int maxAllowed = allowExceed ? int.MaxValue : 250; // No limit if allowed

            if (pageSize < 1 || pageSize > maxAllowed)
            {
                return new ValidationResult($"Page size must be between 1 and {maxAllowed}.");
            }
        }

        return ValidationResult.Success;
    }
}