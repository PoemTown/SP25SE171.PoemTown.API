using PoemTown.Repository.Enums.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.Events.TransactionEvents
{
    public class CreateCommissionTransactionEvent
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public Guid UserEWalletId {  get; set; }
        public Guid PoemId { get; set; }
    }
}
