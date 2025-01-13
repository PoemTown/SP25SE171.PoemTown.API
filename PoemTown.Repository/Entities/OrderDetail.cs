using Microsoft.AspNetCore.Routing.Template;
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
    public class OrderDetail : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public decimal ItemPrice { get; set; }
        public decimal ItemQuantity { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? CopyRightId { get; set; }
        public Guid? TemplateId { get; set; }
        public virtual Order? Order { get; set; }
        public virtual CopyRight? CopyRight { get; set; }
        public virtual Template? Template {  get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public OrderDetail()
        {
            CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        }
    }
}
