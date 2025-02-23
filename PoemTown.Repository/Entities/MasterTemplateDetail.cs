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
using PoemTown.Repository.Enums.TemplateDetails;

namespace PoemTown.Repository.Entities
{
    public class MasterTemplateDetail : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } 
        public string? ColorCode { get; set; }
        public TemplateDetailType? Type { get; set; }
        public TemplateDetailDesignType? DesignType { get; set; }
        public string? Image { get; set; }
        public Guid MasterTemplateId { get; set; }
        
        public virtual MasterTemplate MasterTemplate { get; set; } = null!;
    }
}
