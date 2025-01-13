using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Repository.Entities
{
    public class Comment : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public Guid PoemId { get; set; }
        public Guid AuthorCommentId { get; set; }
        public Guid? ParentCommentId { get; set; }

        [ForeignKey("AuthorCommentId")]
        public virtual User AuthorComment { get; set; }
        [ForeignKey("ParentCommentId")]
        public virtual Comment ParentComment { get; set; }
        public virtual Poem Poem { get; set; }
        public virtual ICollection<Comment> ChildComments { get; set; }

        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public Comment()
        {
            CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        }
    }
}
