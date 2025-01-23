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
using PoemTown.Repository.Enums.Orders;

namespace PoemTown.Repository.Entities
{
    public class Order : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public OrderType? Type { get; set; } = default!;
        public decimal? Amount { get; set; } = 0;
        public DateTimeOffset? OrderDate { get; set; } = DateTimeHelper.SystemTimeNow;
        public string? OrderDescription { get; set; } = null;
        public OrderStatus? Status { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Transaction? Transaction { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
