using System.ComponentModel.DataAnnotations;
using PoemTown.Repository.Base;
using PoemTown.Repository.Enums.MasterTemplates;

namespace PoemTown.Repository.Entities;

public class MasterTemplate : BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public string? TemplateName { get; set; }
    public MasterTemplateStatus? Status { get; set; }
    public decimal Price { get; set; }
    public string? TagName { get; set; } = null;
    public MasterTemplateType? Type { get; set; }
    
    public virtual ICollection<MasterTemplateDetail>? TemplateDetails { get; set; }
    public virtual ICollection<UserTemplate>? Templates { get; set; }
    public virtual OrderDetail? OrderDetail { get; set; }
}