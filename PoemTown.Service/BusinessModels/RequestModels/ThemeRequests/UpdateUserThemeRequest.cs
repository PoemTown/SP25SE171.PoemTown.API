using System.ComponentModel;

namespace PoemTown.Service.BusinessModels.RequestModels.ThemeRequests;

public class UpdateUserThemeRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    [DefaultValue(false)]
    public bool? IsInUse { get; set; } = false;
}