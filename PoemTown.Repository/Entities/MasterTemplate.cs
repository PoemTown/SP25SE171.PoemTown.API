using System.ComponentModel.DataAnnotations;
using PoemTown.Repository.Base;
using PoemTown.Repository.Enums.Templates;

namespace PoemTown.Repository.Entities;

public class MasterTemplate : BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public string? TemplateName { get; set; }
    public TemplateStatus? Status { get; set; }
    public decimal Price { get; set; }
    public string? TagName { get; set; } = null;
    public string? CoverImage { get; set; } 
    public TemplateType? Type { get; set; } = null;
    
    public virtual ICollection<MasterTemplateDetail>? MasterTemplateDetails { get; set; }
    public virtual ICollection<UserTemplate>? Templates { get; set; }
    public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
}