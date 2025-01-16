using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoemTown.Repository.Entities
{
    public class RecordFile : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public Guid PoemId { get; set; } 
        public Guid UserId { get; set; }
        [ForeignKey("PoemId")]
        public virtual Poem? Poem { get; set; }
        [ForeignKey("UserId")]

        public virtual User? User { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }

}
