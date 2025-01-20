using System.ComponentModel.DataAnnotations;
using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class Like : BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid? UserId { get; set; }
    public Guid? PoemId { get; set; }
    public virtual User? User { get; set; }
    public virtual Poem? Poem { get; set; }
}