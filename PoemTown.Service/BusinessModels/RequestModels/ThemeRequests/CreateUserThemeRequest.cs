using System.ComponentModel;

namespace PoemTown.Service.BusinessModels.RequestModels.ThemeRequests;

public class CreateUserThemeRequest
{
    public string Name { get; set; }
    [DefaultValue(false)]
    public bool? IsInUse { get; set; } = false;
    
    [DefaultValue(false)]
    public bool? IsDefault { get; set; } = false;
}