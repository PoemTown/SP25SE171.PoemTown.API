namespace PoemTown.Service.Events.PoemEvents;

public class StorePoemIntoQDrantEvent
{
    public Guid PoemId { get; set; }
    public Guid PoetId { get; set; }
    public string PoemText { get; set; }
    public bool? IsFamousPoem { get; set; } = false;
}