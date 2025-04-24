namespace PoemTown.Service.Events.PoemEvents;

public class DeletePoemPointInQDrantEvent
{
    public IList<Guid>? PoemIds { get; set; }
}