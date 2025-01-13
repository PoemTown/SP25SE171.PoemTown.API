using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Repository.Entities
{
    public class Template : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? TemplateName { get; set; }
        public string? Status { get; set; }
        public decimal? Price { get; set; } 
        public string? TagName { get; set; }
        public string? Type { get; set; }
        public virtual OrderDetail OrderDetail { get; set; }
        public virtual ICollection<TemplateDetail> TemplateDetails { get; set; }
        public virtual ICollection<UserTemplate> UserTemplates { get; set; }


        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public Template()
        {
            CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        }
    }
}
