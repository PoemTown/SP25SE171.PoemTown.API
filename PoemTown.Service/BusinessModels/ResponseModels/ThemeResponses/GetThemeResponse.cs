namespace PoemTown.Service.BusinessModels.ResponseModels.ThemeResponses;

public class GetThemeResponse
{
    public Guid Id { get; set; }
    public bool IsInUse { get; set; }
    public bool IsDefault { get; set; }
    public string Name { get; set; }
}