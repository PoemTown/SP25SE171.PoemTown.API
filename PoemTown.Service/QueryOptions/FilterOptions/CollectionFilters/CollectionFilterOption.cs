using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.Poems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.QueryOptions.FilterOptions.CollectionFilters
{
    public class CollectionFilterOption
    {
        [FromQuery(Name = "collectionName")]
        public string? CollectionName { get; set; }

    }
}
