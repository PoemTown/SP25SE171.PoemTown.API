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
    public class PaymentGateway : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? ImageIcon {  get; set; }
        public bool? IsSuspended { get; set; }

        //public virtual ICollection<Transaction>? Transactions { get; set; }  
        //public virtual ICollection<Order>? Orders { get; set; }  

    }
}
