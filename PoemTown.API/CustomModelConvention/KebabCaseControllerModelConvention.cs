using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace PoemTown.API.CustomModelConvention;

public class KebabCaseControllerModelConvention : IApplicationModelConvention
{
    public void Apply(ApplicationModel appModel)
    {
        foreach (var controller in appModel.Controllers)
        {
            // Convert controller name to kebab-case
            string kebabCaseName = ConvertToKebabCase(controller.ControllerName);

            foreach (var selector in controller.Selectors)
            {
                // Update the template to use kebab-case
                if (selector.AttributeRouteModel != null)
                {
                    var template = selector.AttributeRouteModel.Template;

                    // Replace [controller] with kebab-case controller name
                    template = template.Replace("[controller]", kebabCaseName);
                    
                    // Set the updated template back to the selector
                    selector.AttributeRouteModel.Template = template;
                }
            }
        }
    }

    private string ConvertToKebabCase(string name)
    {
        return Regex.Replace(name, "([a-z])([A-Z])", "$1-$2") // Add hyphen before capital letters
            .Replace("_", "-") // Replace underscores with hyphens
            .ToLowerInvariant(); // Convert to lowercase
    }
}