using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.Poems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.QueryOptions.FilterOptions.PoemFilters
{
    public class GetPoemsFilterOption
    {
        /*[FromQuery(Name = "collectionId")]
        public Guid? CollectionId { get; set; }*/

        [FromQuery(Name = "chapterName")]
        public string? ChapterName { get; set; }

        [FromQuery(Name = "title")]
        public string? Title { get; set; }

        [FromQuery(Name = "type")]
        public PoemType? Type { get; set; }

        [FromQuery(Name = "status")]
        public PoemStatus? Status { get; set; }

        [FromQuery(Name = "audio")]
        public PoemAudio? AudioStatus { get; set; }
    }
}
