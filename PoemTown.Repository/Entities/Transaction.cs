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
    public class Transaction : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }    
        public string? Name { get; set; }    
        public string? Description { get; set; }
        public string? Status { get; set; }
        public decimal? BalanceBefore { get; set; }
        public decimal? BalacneAfter { get; set; }
        public decimal? Amount {  get; set; }

        public Guid UserId { get; set; }
        public Guid PayementGatewayId { get; set; }
        public Guid UserEWalletId { get; set; }
        public Guid OrderId {  get; set; }

        public virtual User User { get; set; }
        public virtual PaymentGateway PayementGateway { get; set; }
        public virtual UserEWallet EWallet { get; set; }
        public virtual Order Order { get; set; }



        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        public Transaction()
        {
            CreatedTime = LastUpdatedTime = DateTimeHelper.SystemTimeNow;
        }
    }
}
