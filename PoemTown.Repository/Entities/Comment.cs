using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities
{
    public class Comment : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Content { get; set; } = default!;
        public Guid PoemId { get; set; }
        public Guid AuthorCommentId { get; set; }
        public Guid? ParentCommentId { get; set; }

        [ForeignKey("AuthorCommentId")]
        public virtual User AuthorComment { get; set; }

        [ForeignKey("ParentCommentId")]
        public virtual Comment? ParentComment { get; set; } = null;
        public virtual Poem Poem { get; set; }
        public virtual ICollection<Announcement>? Announcements { get; set; }
        public virtual ICollection<Comment>? ChildComments { get; set; }

    }
}
