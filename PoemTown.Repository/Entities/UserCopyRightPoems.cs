/*using System.ComponentModel.DataAnnotations;
using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities;

public class UserCopyRightPoems : BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid? UserCopyRightId { get; set; }
    public Guid? PoemId { get; set; }
    
    public virtual UserCopyRight? UserCopyRight { get; set; }
    public virtual Poem? Poem { get; set; }
}*/