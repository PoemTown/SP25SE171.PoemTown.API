using System.ComponentModel.DataAnnotations;

namespace PoemTown.Service.QueryOptions.ExtensionOptions;

public class TimeRange : IValidatableObject
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (From > To)
        {
            yield return new ValidationResult("'From' date cannot be greater than 'To' date.");
        }
    }
}