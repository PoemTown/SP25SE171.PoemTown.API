using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Base;
using PoemTown.Repository.Enums;

namespace PoemTown.Repository.Entities
{
    public class TemplateDetail : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } 
        public string? DesignContent { get; set; }
        public TemplateDetailType? Type { get; set; }
        public Guid TemplateId { get; set; }
        public virtual Template Template { get; set; }
    }
}
