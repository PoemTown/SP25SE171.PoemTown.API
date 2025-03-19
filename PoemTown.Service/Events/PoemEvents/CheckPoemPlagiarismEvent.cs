namespace PoemTown.Service.Events.PoemEvents;

public class CheckPoemPlagiarismEvent
{
    public Guid PoemId { get; set; }
    public Guid UserId { get; set; }
    public string PoemContent { get; set; }
}