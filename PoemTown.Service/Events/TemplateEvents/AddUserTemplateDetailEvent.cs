using PoemTown.Repository.Entities;

namespace PoemTown.Service.Events.TemplateEvents;

public class AddUserTemplateDetailEvent
{
    public Guid MasterTemplateId { get; set; }
    public IEnumerable<Guid> MasterTemplateDetailIds { get; set; }
}