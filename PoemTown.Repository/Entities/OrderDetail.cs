using Microsoft.AspNetCore.Routing.Template;
using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities
{
    public class OrderDetail : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public decimal ItemPrice { get; set; } = default;
        public decimal ItemQuantity { get; set; } = default;
        public Guid OrderId { get; set; }
        public Guid? CopyRightId { get; set; }
        public Guid? TemplateId { get; set; }
        public virtual Order Order { get; set; } = default!;
        public virtual CopyRight? CopyRight { get; set; }
        public virtual Template? Template {  get; set; }
    }
}
