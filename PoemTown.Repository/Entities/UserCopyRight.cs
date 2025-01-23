/*
using PoemTown.Repository.Base.Interfaces;
using PoemTown.Repository.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Repository.Base;

namespace PoemTown.Repository.Entities
{
    public class UserCopyRight : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime ExperiedTime { get; set; }  
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
*/
