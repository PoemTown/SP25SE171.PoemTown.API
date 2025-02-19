using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class ThemeUserTemplateDetail : BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public Guid ThemeId { get; set; }
    public Guid UserTemplateDetailId { get; set; }
    public virtual Theme Theme { get; set; } = null!;
    public virtual UserTemplateDetail UserTemplateDetail { get; set; } = null!;
}