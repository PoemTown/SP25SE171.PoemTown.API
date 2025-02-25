using PoemTown.Repository.Base;
using PoemTown.Repository.Enums.TemplateDetails;

namespace PoemTown.Repository.Entities;

public class UserTemplateDetail : BaseEntity
{
    public Guid Id { get; set; }
    //public string? ColorCode { get; set; }
    public string? ColorCode { get; set; }
    public TemplateDetailType? Type { get; set; }
    public string? Image { get; set; }
    //public TemplateDetailDesignType? DesignType { get; set; }

    //public Guid? ParentTemplateDetailId { get; set; }
    public Guid UserTemplateId { get; set; }
    
    //public virtual UserTemplateDetail? ParentTemplateDetail { get; set; } = null!;
    public virtual UserTemplate UserTemplate { get; set; } = null!;
    public virtual ICollection<ThemeUserTemplateDetail>? ThemeUserTemplateDetails { get; set; }
}