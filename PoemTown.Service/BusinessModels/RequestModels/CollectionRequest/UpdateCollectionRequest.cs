using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.RequestModels.CollectionRequest
{
    public class UpdateCollectionRequest
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Collection name is required.")]
        [MaxLength(100, ErrorMessage = "Collection name must be at most 100 characters.")]
        public string CollectionName { get; set; } = "";

        [MaxLength(500, ErrorMessage = "Description must be at most 500 characters.")]
        public string CollectionDescription { get; set; } = "";
        public string CollectionImage { get; set; } = null;
    }
}
