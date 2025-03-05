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
using System.ComponentModel.DataAnnotations.Schema;

namespace PoemTown.Repository.Entities
{
    public class OrderDetail : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public decimal ItemPrice { get; set; } = default;
        public int ItemQuantity { get; set; } = default;
        public Guid OrderId { get; set; }
        public Guid? PoemId { get; set; }
        public Guid? MasterTemplateId { get; set; }
        public Guid? RecordFileId { get; set; }
        public Guid? UserEWalletId { get; set; }
        public virtual Order Order { get; set; } = default!;
        public virtual RecordFile? RecordFile { get; set; }
        public virtual Poem? Poem { get; set; }
        public virtual UserEWallet? UserEWallet { get; set; }
        public virtual MasterTemplate? MasterTemplate {  get; set; }
    }
}
