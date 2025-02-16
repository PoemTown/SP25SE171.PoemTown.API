using PoemTown.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.ResponseModels.CollectionResponses
{
    public class GetCollectionResponse
    {
        public Guid Id { get; set; }
        public string CollectionName { get; set; } = default!;
        public string? CollectionDescription { get; set; } = default!;
        public string? CollectionImage { get; set; } = default!;
        /*public bool? IsDefault { get; set; } = false;*/
        public int? TotalChapter { get; set; } = default!;
        /*public virtual ICollection<TargetMark>? TargetMarks { get; set; } = null;*/
    }
}
