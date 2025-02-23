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
using PoemTown.Repository.Enums.Templates;

namespace PoemTown.Repository.Entities
{
    public class UserTemplate : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string? TemplateName { get; set; } = "";
        public TemplateStatus? Status { get; set; }
        public string? TagName { get; set; } = "";
        public TemplateType? Type { get; set; }
        public Guid? MasterTemplateId { get; set; }
        public Guid UserId { get; set; }
        
        public virtual User? User { get; set; }
        public virtual MasterTemplate? MasterTemplate { get; set; }
        /*
        public virtual OrderDetail? OrderDetail { get; set; }
        */
        /*
        public virtual ICollection<UserTemplate>? UserTemplates { get; set; }
        */
        
    }
}
