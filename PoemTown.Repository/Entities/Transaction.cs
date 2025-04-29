using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Base;
using PoemTown.Repository.Enums.Transactions;

namespace PoemTown.Repository.Entities
{
    public class Transaction : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        //public string? Name { get; set; } = "";
        public string? TransactionCode { get; set; }
        public string? Description { get; set; } = "";
        //public string? Status { get; set; } = "";
        public decimal? Balance { get; set; } = default;
        public decimal Amount { get; set; }
        public string? Token { get; set; }
        public string? AppId { get; set; }
        public string? BankCode { get; set; }
        public string? Checksum { get; set; }
        public string? TransactionToken { get; set; }
        public TransactionType Type { get; set; }
        public decimal? DiscountAmount { get; set; }
        public DateTimeOffset? PaidDate { get; set; }
        public DateTimeOffset? CancelledDate { get; set; }
        public TransactionStatus? Status { get; set; }
        public bool? IsAddToWallet { get; set; }
        //public Guid? UserId { get; set; }
        //public Guid? PaymentGatewayId { get; set; }
        public Guid? UserEWalletId { get; set; }
        public Guid? ReceiveUserEWalletId { get; set; }
        public Guid? OrderId {  get; set; }
        public Guid? PaymentGatewayId { get; set; }

        //public virtual User? User { get; set; }
        //public virtual PaymentGateway? PaymentGateway { get; set; }
        public virtual UserEWallet? UserEWallet { get; set; }
        public virtual UserEWallet? ReceiveUserEWallet { get; set; }
        public virtual PaymentGateway? PaymentGateway { get; set; }
        public virtual Order? Order { get; set; }
        public virtual ICollection<Announcement>? Announcements { get; set; }
        
        public Guid? DepositCommissionSettingId { get; set; }
        public virtual DepositCommissionSetting? DepositCommissionSetting { get; set; }
    }
}
