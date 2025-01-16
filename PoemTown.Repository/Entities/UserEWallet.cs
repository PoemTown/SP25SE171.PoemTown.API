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
    public class UserEWallet : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public decimal WalletBalance { get; set; }
        public WalletStatus WalletStatus { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
