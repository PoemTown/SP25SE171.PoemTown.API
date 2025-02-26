namespace PoemTown.Service.Events.ThemeEvents;

public class CreateDefaultUserThemeEvent
{
    public Guid UserId { get; set; }
    public bool IsInUse { get; set; }
}